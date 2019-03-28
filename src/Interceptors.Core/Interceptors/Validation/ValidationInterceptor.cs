using Abp.Dependency;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Interceptors.Interceptors.Validation
{
    public class ValidationInterceptor : IInterceptor
    {

        private readonly IIocResolver _iocResolver;

        public ILogger Logger { get; set; }

        public ValidationInterceptor(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// Méthode qui vérifie si on doit intercepter les invocation fait sur IBaseApplicationService
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns>Retourne si on doit intercepter ou non</returns>
        private bool ShouldIntercept(IInvocation invocation)
        {
            return invocation.MethodInvocationTarget.GetCustomAttribute<UxValidationDisabledAttribute>() == null;
        }

        /// <summary>
        /// Méthode qui est appelé avant de rentré dans les méthode des IBaseApplicationService
        /// N.b votre méthode doit être virtual pour être intercepté
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {

            if (ShouldIntercept(invocation))
            {
                ValidateBeforeProceeding(invocation);
            }
            else
            {
                invocation.Proceed();
            }
        }

        private void ValidateBeforeProceeding(IInvocation invocation)
        {
            //le invocation vas être destiné (target) a un IBaseValidatedAppService, car dans le ValidationRegistrar dans la méthode Kernel_ComponentRegistered
            //seulement les IBaseRestApplicationService sont handle par l'Interceptor.
            IBaseValidatedAppService appService = invocation.InvocationTarget as IBaseValidatedAppService;

            string assemblyName = invocation.InvocationTarget.GetType().BaseType.Assembly.ManifestModule.Name.Replace(".dll", ".");

            string validatorName = "I" + appService.GetType().BaseType.Name + "Validator";

            TypeResolver typeResolver = _iocResolver.Resolve<TypeResolver>();

            Type validatorInterfaceType = typeResolver[assemblyName + validatorName];

            if (validatorInterfaceType is null)
                return;

            IBaseValidator baseValidator = _iocResolver.Resolve(validatorInterfaceType) as IBaseValidator;

            Type validatorType = baseValidator.GetType();

            //IocManager.Instance.IocContainer.Resolve("");
            //on vas essayer d'aller chercher par réflection les méthode de validation
            //on vas devoir avoir un standard que les méthode dans les Validator qui hérite de IBaseValidation
            //doivent avoir le même nom que la méthode du app service qu'elle valide plus le terme Validation
            string methodName = invocation.MethodInvocationTarget.Name + "Validation";

            MethodInfo method = validatorType.GetMethod(methodName);

            if (method != null)
            {
                //on invoke la méthode du validator
                //on doit faire le try catch et le re-throw ici sinon on perdait le type de l'exception
                try
                {
                    if (InternalAsyncHelper.IsAsyncMethod(method))
                    {
                        var returnValue = method.Invoke(baseValidator, invocation.Arguments);
                        ////Wait task execution and modify return value
                        if (method.ReturnType == typeof(Task))
                        {
                            returnValue = InternalAsyncHelper.AwaitTaskWithFinally(
                                (Task)returnValue,
                                ex =>
                                {
                                    invocation.Proceed();
                                });
                        }
                        else //Task<TResult>
                        {
                            returnValue = InternalAsyncHelper.CallAwaitTaskWithFinallyAndGetResult(
                                method.ReturnType.GenericTypeArguments[0],
                                returnValue,
                                ex =>
                                {
                                    invocation.Proceed();
                                });
                        }
                    }
                    else
                    {
                        invocation.Proceed();
                    }
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }
            else
            {
            }

        }

    }
}

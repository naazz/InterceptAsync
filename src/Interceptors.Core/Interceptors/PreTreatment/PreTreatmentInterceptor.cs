using Abp.Dependency;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Interceptors.Interceptors.PreTreatment
{
    public class PreTreatmentInterceptor : IInterceptor
    {
        private readonly IIocResolver _iocResolver;

        public ILogger Logger { get; set; }

        public PreTreatmentInterceptor(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
            Logger = NullLogger.Instance;
        }

        private bool ShouldIntercept(IInvocation invocation)
        {
            return invocation.MethodInvocationTarget.GetCustomAttribute<UxPreTreatment>() != null;
        }

        public void Intercept(IInvocation invocation)
        {
            if (ShouldIntercept(invocation))
            {
                PreTreatmentBeforeProceeding(invocation);
            }
            else
            {
                invocation.Proceed();
            }
        }

        private void PreTreatmentBeforeProceeding(IInvocation invocation)
        {
            IPreTreatmentAppService appService = invocation.InvocationTarget as IPreTreatmentAppService;

            string assemblyName = invocation.InvocationTarget.GetType().BaseType.Assembly.ManifestModule.Name.Replace(".dll", ".");

            string preTreatmentExecutorName = "I" + appService.GetType().BaseType.Name + "PreTreatmentExecutor";

            TypeResolver typeResolver = _iocResolver.Resolve<TypeResolver>();

            Type preTreatmentExecutorInterfaceType = typeResolver[assemblyName + preTreatmentExecutorName];

            if (preTreatmentExecutorInterfaceType is null)
                return;

            IPreTreatmentExecutor preTreatmentExecutor = _iocResolver.Resolve(preTreatmentExecutorInterfaceType) as IPreTreatmentExecutor;

            Type preTreatmentExecutorType = preTreatmentExecutor.GetType();

            string methodName = "PreTreatment_" + invocation.MethodInvocationTarget.Name;

            MethodInfo method = preTreatmentExecutorType.GetMethod(methodName);

            var request = invocation.Arguments[0];


            if (method != null)
            {
                try
                {
                    var returnValue = method.Invoke(preTreatmentExecutor, invocation.Arguments);

                    if (InternalAsyncHelper.IsAsyncMethod(method))
                    {
                        //Wait task execution and modify return value
                        if (method.ReturnType == typeof(Task))
                        {
                            invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithFinally(
                          (Task)returnValue,
                               ex =>
                               {
                                   invocation.Proceed();
                               });
                        }
                        else //Task<TResult>
                        {
                            invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithFinallyAndGetResult(
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
        }
    }
}

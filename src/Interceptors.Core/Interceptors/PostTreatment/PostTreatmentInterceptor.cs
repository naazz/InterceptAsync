using Abp.Dependency;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Interceptors.Interceptors.PostTreatment
{
    public class PostTreatmentInterceptor : IInterceptor
    {
        private readonly IIocResolver _iocResolver;

        public PostTreatmentInterceptor(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }
        public void Intercept(IInvocation invocation)
        {
            if (ShouldIntercept(invocation))
            {
                Intercept(invocation, InternalAsyncHelper.IsAsyncMethod(invocation.Method));
            }
            else
            {
                invocation.Proceed();
            }
        }

        /// <summary>
        /// Méthode qui vérifie si on doit intercepter les invocation fait sur IPostTreatmentAppService
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns>Retourne si on doit intercepter ou non</returns>
        private bool ShouldIntercept(IInvocation invocation)
        {
            if (invocation.MethodInvocationTarget.GetCustomAttribute<UxPostTreatment>() != null)
            {
                return true;
            }

            //par défaut on vas intercepter aucune des invocation fait sur les IPostTreatmentAppService
            return false;
        }

        private void Intercept(IInvocation invocation, bool pIsAsync)
        {
            PreTreatmentBeforeInvocation(invocation, pIsAsync);
        }

        private void PreTreatmentBeforeInvocation(IInvocation invocation, bool pIsAsync)
        {
            Type type = invocation.TargetType.GetInterfaces().Single(w => w.IsGenericType && w.GetGenericTypeDefinition() == typeof(IPostTreatmentAppService<>));

            Type implementedType = type.GetGenericArguments()[0];

            MethodInfo genericPreTreatmentBeforeMethodInfo = this.GetType().GetMethod(nameof(GenericPreTreatmentBeforeInvocation), BindingFlags.NonPublic | BindingFlags.Instance)
                                                                           .MakeGenericMethod(implementedType);

            genericPreTreatmentBeforeMethodInfo.Invoke(this, new object[] { invocation, pIsAsync });
        }

        private void GenericPreTreatmentBeforeInvocation<T>(IInvocation invocation, bool pIsAsync)
        {
            this.ProceedAndCallPostTreatment<T>(invocation, new List<T>(), pIsAsync);
        }

        private void ProceedAndCallPostTreatment<T>(IInvocation invocation, List<T> pEntities, bool IsAsync)
        {
            invocation.Proceed();


            if (IsAsync)
            {
                ////Wait task execution and modify return value
                if (invocation.Method.ReturnType == typeof(Task))
                {
                    invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                        (Task)invocation.ReturnValue,
                        async () =>
                        {
                            GenericPostTreatment(invocation, pEntities);
                        },
                        ex =>
                        {

                        });
                }
                else //Task<TResult>
                {
                    invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                        async (e) =>
                        {
                            invocation.ReturnValue = e;
                            GenericPostTreatment(invocation, pEntities);
                        },
                        ex =>
                        {

                        });
                }
            }
            else
            {
                this.GenericPostTreatment(invocation, pEntities);
            }
        }

        private void GenericPostTreatment<T>(IInvocation invocation, List<T> pEntities)
        {
            Task task = invocation.ReturnValue as Task;

            if (task != null && task.Status != TaskStatus.Faulted)
            {
                MethodInfo genericPostTreatment = this.GetType().GetMethod(nameof(InnerGenericPostTreatment), BindingFlags.NonPublic | BindingFlags.Instance)
                                                                .MakeGenericMethod(typeof(T));

                genericPostTreatment.Invoke(this, new object[] { invocation, pEntities });
            }
        }

        private void InnerGenericPostTreatment<TEntity>(IInvocation invocation, List<TEntity> pEntities)
        {
            string assemblyName = invocation.InvocationTarget.GetType().BaseType.Assembly.ManifestModule.Name.Replace(".dll", ".");

            string validatorName = "I" + invocation.InvocationTarget.GetType().BaseType.Name + "PostTreatmentExecutor";

            TypeResolver typeResolver = _iocResolver.Resolve<TypeResolver>();

            Type validatorInterfaceType = typeResolver[assemblyName + validatorName];

            if (validatorInterfaceType is null)
                return;

            IPostTreatmentExecutor<TEntity> postTreatmentExecutor = _iocResolver.Resolve(validatorInterfaceType) as IPostTreatmentExecutor<TEntity>;

            Type executorType = postTreatmentExecutor.GetType();

            string methodName = "PostTreatment_" + invocation.MethodInvocationTarget.Name;

            MethodInfo method = executorType.GetMethod(methodName);

            var request = invocation.Arguments[0];

            var result = invocation.ReturnValue.GetType().GetProperty("Result").GetValue(invocation.ReturnValue);

            var infos = new object[] { request, result };

            method.Invoke(postTreatmentExecutor, infos);
        }
    }
}


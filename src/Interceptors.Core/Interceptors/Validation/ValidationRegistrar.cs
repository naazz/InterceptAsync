using Abp.Application.Services;
using Abp.Runtime.Validation.Interception;
using Castle.Core;
using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interceptors.Interceptors.Validation
{
    public static class ValidationRegistrar
    {
        public static void Initialize(IKernel kernel)
        {
            kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private static void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            if (typeof(IBaseValidatedAppService).IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(ValidationInterceptor)));
            }
        }
    }
}

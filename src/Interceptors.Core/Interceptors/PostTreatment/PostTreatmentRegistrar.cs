using Castle.Core;
using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interceptors.Interceptors.PostTreatment
{
    public static class PostTreatmentRegistrar
    {

        public static void Initialize(IKernel kernel)
        {
            kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private static void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            if(handler.ComponentModel.Implementation.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IPostTreatmentAppService<>)))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(PostTreatmentInterceptor)));
            }
        }

    }
}

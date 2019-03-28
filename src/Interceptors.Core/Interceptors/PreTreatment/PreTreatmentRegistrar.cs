using Castle.Core;
using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interceptors.Interceptors.PreTreatment
{
    public class PreTreatmentRegistrar
    {
        public static void Initialize(IKernel kernel)
        {
            kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private static void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            if (typeof(IPreTreatmentAppService).IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(PreTreatmentInterceptor)));
            }
        }
    }
}

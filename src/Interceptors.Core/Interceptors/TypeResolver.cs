using Interceptors.Interceptors.PostTreatment;
using Interceptors.Interceptors.PreTreatment;
using Interceptors.Interceptors.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Interceptors.Interceptors
{
    public class TypeResolver
    {
        private Dictionary<string, Type> Types { get; set; }

        public TypeResolver()
        {
            Types = new Dictionary<string, Type>();
        }

        public Type this[string pKey]
        {
            get
            {
                if (Types.ContainsKey(pKey))
                {
                    return Types[pKey];
                }

                return null;
            }
        }

        public void RegisterTypes(Assembly pAssembly)
        {
            List<Type> types = pAssembly.DefinedTypes.Where(w => typeof(IBaseValidator).IsAssignableFrom(w) ||
                                                                 w.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IPostTreatmentExecutor<>)) ||
                                                                 typeof(IPreTreatmentExecutor).IsAssignableFrom(w)
                                                            )
                                                     .Select(s => s.UnderlyingSystemType).ToList();

            string assemblyName = pAssembly.ManifestModule.Name.Replace(".dll", ".");

            foreach (Type type in types)
            {
                Types.Add(assemblyName + type.Name, type);
            }
        }
    }
}
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Interceptors.Authorization;
using Interceptors.Interceptors;

namespace Interceptors
{
    [DependsOn(
        typeof(InterceptorsCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class InterceptorsApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<InterceptorsAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(InterceptorsApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );

            this.IocManager.Resolve<TypeResolver>().RegisterTypes(thisAssembly);
        }
    }
}

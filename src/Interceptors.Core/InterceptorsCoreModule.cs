using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Interceptors.Authorization.Roles;
using Interceptors.Authorization.Users;
using Interceptors.Configuration;
using Interceptors.Interceptors;
using Interceptors.Interceptors.PostTreatment;
using Interceptors.Interceptors.PreTreatment;
using Interceptors.Interceptors.Validation;
using Interceptors.Localization;
using Interceptors.MultiTenancy;
using Interceptors.Timing;

namespace Interceptors
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class InterceptorsCoreModule : AbpModule
    {
        public override void PreInitialize()
        {

            this.IocManager.IocContainer.Register(Component.For<TypeResolver>().UsingFactoryMethod(() => new TypeResolver()).LifeStyle.Singleton);

            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            InterceptorsLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = InterceptorsConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            ValidationRegistrar.Initialize(this.IocManager.IocContainer.Kernel);
            PreTreatmentRegistrar.Initialize(this.IocManager.IocContainer.Kernel);
            PostTreatmentRegistrar.Initialize(this.IocManager.IocContainer.Kernel);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InterceptorsCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}

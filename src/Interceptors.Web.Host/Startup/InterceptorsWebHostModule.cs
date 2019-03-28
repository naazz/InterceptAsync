using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Interceptors.Configuration;

namespace Interceptors.Web.Host.Startup
{
    [DependsOn(
       typeof(InterceptorsWebCoreModule))]
    public class InterceptorsWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public InterceptorsWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InterceptorsWebHostModule).GetAssembly());
        }
    }
}

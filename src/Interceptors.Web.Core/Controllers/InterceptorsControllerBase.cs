using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Interceptors.Controllers
{
    public abstract class InterceptorsControllerBase: AbpController
    {
        protected InterceptorsControllerBase()
        {
            LocalizationSourceName = InterceptorsConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}

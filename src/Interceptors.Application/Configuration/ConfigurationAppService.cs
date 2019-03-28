using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Interceptors.Configuration.Dto;

namespace Interceptors.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : InterceptorsAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}

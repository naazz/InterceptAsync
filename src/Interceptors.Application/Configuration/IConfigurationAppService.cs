using System.Threading.Tasks;
using Interceptors.Configuration.Dto;

namespace Interceptors.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}

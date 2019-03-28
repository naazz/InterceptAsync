using System.Threading.Tasks;
using Abp.Application.Services;
using Interceptors.Authorization.Accounts.Dto;

namespace Interceptors.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}

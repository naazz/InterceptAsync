using System.Threading.Tasks;
using Abp.Application.Services;
using Interceptors.Sessions.Dto;

namespace Interceptors.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}

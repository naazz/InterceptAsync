using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Interceptors.MultiTenancy.Dto;

namespace Interceptors.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}


using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Joos.MultiTenancy.Dto;

namespace Joos.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultOutput<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}

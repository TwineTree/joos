using System.Threading.Tasks;
using Abp.Application.Services;
using Joos.Roles.Dto;

namespace Joos.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}

using System.Threading.Tasks;
using Abp.Application.Services;
using Joos.Users.Dto;

namespace Joos.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task TestTask(long userId);
    }
}
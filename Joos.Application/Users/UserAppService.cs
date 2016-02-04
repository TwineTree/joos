using System.Threading.Tasks;
using Abp.Authorization;
using Joos.Users.Dto;

namespace Joos.Users
{
    /* THIS IS JUST A SAMPLE. */
    public class UserAppService : JoosAppServiceBase, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly IPermissionManager _permissionManager;

        public UserAppService(UserManager userManager, IPermissionManager permissionManager)
        {
            _userManager = userManager;
            _permissionManager = permissionManager;
        }

        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await _userManager.ProhibitPermissionAsync(user, permission);
        }

        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {
            CheckErrors(await _userManager.RemoveFromRoleAsync(userId, roleName));
        }

        public async Task TestTask(long userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);

            user.Name = user.Name + "Edit";

            await _userManager.UpdateAsync(user);
        }
    }
}
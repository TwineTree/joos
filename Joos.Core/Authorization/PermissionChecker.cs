using Abp.Authorization;
using Joos.Authorization.Roles;
using Joos.MultiTenancy;
using Joos.Users;

namespace Joos.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}

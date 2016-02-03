using Abp.Authorization.Roles;
using Joos.MultiTenancy;
using Joos.Users;

namespace Joos.Authorization.Roles
{
    public class Role : AbpRole<Tenant, User>
    {

    }
}
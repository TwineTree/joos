using Abp.Application.Features;
using Joos.Authorization.Roles;
using Joos.MultiTenancy;
using Joos.Users;

namespace Joos.Features
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(TenantManager tenantManager)
            : base(tenantManager)
        {
        }
    }
}
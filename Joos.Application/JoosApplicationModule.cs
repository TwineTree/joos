using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;

namespace Joos
{
    [DependsOn(typeof(JoosCoreModule), typeof(AbpAutoMapperModule))]
    public class JoosApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

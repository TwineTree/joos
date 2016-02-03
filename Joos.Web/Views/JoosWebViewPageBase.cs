using Abp.Web.Mvc.Views;

namespace Joos.Web.Views
{
    public abstract class JoosWebViewPageBase : JoosWebViewPageBase<dynamic>
    {

    }

    public abstract class JoosWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected JoosWebViewPageBase()
        {
            LocalizationSourceName = JoosConsts.LocalizationSourceName;
        }
    }
}
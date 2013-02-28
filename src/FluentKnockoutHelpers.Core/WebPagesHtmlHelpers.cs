using System.Web.WebPages;
using System.Web.WebPages.Html;
using FluentKnockoutHelpers.Core.Builders;

namespace FluentKnockoutHelpers.Core
{
    public static class WebPagesHtmlHelpers
    {
        public static Builder<TKoModel> KnockoutHelperForType<TKoModel>(this WebPageBase @this)
        {
            return new Builder<TKoModel>(@this);
        }

        public static Builder<TKoModel> KnockoutHelperForType<TKoModel>(this WebPageBase @this, string viewModelPropertyName, bool viewModelPropertyIsObservable)
        {
            return new Builder<TKoModel>(@this, viewModelPropertyName + (viewModelPropertyIsObservable ? "()" : string.Empty));
        }
    }
}
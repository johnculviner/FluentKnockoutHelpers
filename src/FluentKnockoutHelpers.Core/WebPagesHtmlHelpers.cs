using System.Web.WebPages.Html;
using FluentKnockoutHelpers.Core.Builders;

namespace FluentKnockoutHelpers.Core
{
    public static class WebPagesHtmlHelpers
    {
        public static Builder<TKoModel> KnockoutHelperForType<TKoModel>(this HtmlHelper @this)
        {
            return new Builder<TKoModel>(new WebPagesHtmlHelperAdapter(@this));
        }

        public static Builder<TKoModel> KnockoutHelperForType<TKoModel>(this HtmlHelper @this, string viewModelPropertyName)
        {
            return new Builder<TKoModel>(new WebPagesHtmlHelperAdapter(@this), viewModelPropertyName);
        }
    }
}
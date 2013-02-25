using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentKnockoutHelpers.Core.Builders;
using FluentKnockoutHelpers.Mvc.Builders;

namespace FluentKnockoutHelpers.Mvc
{
    public static class MvcHtmlHelpers
    {
        public static Builder<TKoModel> KnockoutHelperForType<TKoModel>(this HtmlHelper @this)
        {
            return new Builder<TKoModel>(new MvcHtmlHelperAdapter(@this));
        }

        public static Builder<TKoModel> KnockoutHelperForType<TKoModel>(this HtmlHelper @this, string viewModelPropertyName)
        {
            return new Builder<TKoModel>(new MvcHtmlHelperAdapter(@this), viewModelPropertyName);
        }

        public static InstanceBuilder<TModel, TKoModel> KnockoutHelperForInstance<TModel, TKoModel>(this HtmlHelper<TModel> @this, Func<TModel, TKoModel> instanceExpr)
        {
            return new InstanceBuilder<TModel, TKoModel>(@this, instanceExpr);
        }

        public static InstanceBuilder<TModel, TKoModel> KnockoutHelperForInstance<TModel, TKoModel>(this HtmlHelper<TModel> @this, Func<TModel, TKoModel> instanceExpr, string viewModelPropertyName)
        {
            return new InstanceBuilder<TModel, TKoModel>(@this, instanceExpr, viewModelPropertyName);
        }
    }
}
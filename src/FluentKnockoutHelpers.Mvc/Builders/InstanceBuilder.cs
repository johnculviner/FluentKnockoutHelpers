using System;
using System.Web;
using System.Web.Mvc;
using FluentKnockoutHelpers.Core;
using FluentKnockoutHelpers.Core.Builders;

namespace FluentKnockoutHelpers.Mvc.Builders
{
    //TODO maybe bring this back or delete it
    //public class InstanceBuilder<TModel, TKoModel> : Builder<TKoModel>
    //{
    //    private TKoModel _instance;

    //    public InstanceBuilder(HtmlHelper<TModel> htmlHelper, Func<TModel, TKoModel> instanceExpr)
    //        : this(htmlHelper, instanceExpr, null)
    //    { 
    //    }

    //    public InstanceBuilder(HtmlHelper<TModel> htmlHelper, Func<TModel, TKoModel> instanceExpr, string viewModelPropertyName)
    //        : base(new MvcHtmlHelperAdapter(htmlHelper), viewModelPropertyName)
    //    {
    //        _instance = instanceExpr(htmlHelper.ViewData.Model);
    //        }

    //    public HtmlString InstanceToJson()
    //    {
    //        return new HtmlString(GlobalSettings.JsonSerializer.ToJsonString(_instance));
    //    }
    //}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using FluentKnockoutHelpers.Core;
using FluentKnockoutHelpers.Core.Builders;
using FluentKnockoutHelpers.Core.Utility;

namespace SurveyApp.Web.Helpers
{
    public static class GeneralHelpers
    {
        /// <summary>
        /// Demos how easy it is to create a custom helper:
        /// renders a datepicker binding with a twitterbootstrap style
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="this"></param>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public static StringReturningBuilder<TModel> BoundDatePickerFor<TModel, TProp>(this Builder<TModel> @this, Expression<Func<TModel, TProp>> propExpr)
        {
            return @this.ElementSelfClosing("input").Class("input-small").Attr("type", "text").DataBind(db => db.Custom("datepicker", propExpr));
        }

        public static StringReturningBuilder<TModel> BootstrapLabelFor<TModel, TProp>(this Builder<TModel> @this, Expression<Func<TModel, TProp>> propExpr)
        {
            return @this.LabelFor(propExpr).Class("control-label");
        }
    }
}

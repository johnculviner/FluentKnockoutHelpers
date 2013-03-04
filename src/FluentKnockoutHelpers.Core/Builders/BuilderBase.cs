using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;
using FluentKnockoutHelpers.Core.Utility;

namespace FluentKnockoutHelpers.Core.Builders
{
    public class BuilderBase<TModel>
    {
        public string ViewModelPropertyName { get; private set; }
        public WebPageBase WebPage { get; private set; }

        #region ctor

        public BuilderBase(WebPageBase webPage)
            : this(webPage, null)
        {
        }

        public BuilderBase(WebPageBase webPage, string viewModelPropertyName)
        {
            WebPage = webPage;
            ViewModelPropertyName = viewModelPropertyName;
        }

        protected BuilderBase(BuilderBase<TModel> builder)
        {
            ViewModelPropertyName = builder.ViewModelPropertyName;
            WebPage = builder.WebPage;
        }
        #endregion


        ///// <summary>
        ///// Emits a string that will be derived from the [Display..] attribute or the property name of the passed 'propExpr'
        ///// </summary>
        ///// <typeparam name="TProp"></typeparam>
        ///// <param name="propExpr"></param>
        ///// <returns></returns>
        public virtual IHtmlString DisplayNameFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return new HtmlString(ExpressionParser.DisplayNameFor(propExpr));
        }
    }
}

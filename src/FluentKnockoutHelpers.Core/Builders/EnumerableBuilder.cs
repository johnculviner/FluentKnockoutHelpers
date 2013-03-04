using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;
using FluentKnockoutHelpers.Core.NodeBuilding;
using FluentKnockoutHelpers.Core.Utility;

namespace FluentKnockoutHelpers.Core.Builders
{
    public class EnumerableBuilder<TModel> : BuilderBase<TModel>
    {
        public EnumerableBuilder(WebPageBase webPage) : base(webPage)
        {
        }

        public EnumerableBuilder(WebPageBase webPage, string viewModelPropertyName) : base(webPage, viewModelPropertyName)
        {
        }

        public EnumerableBuilder(BuilderBase<TModel> builder) : base(builder)
        {
        }

        public ForEachBuilder<TModel> ForEachKoComment()
        {
            return ForEach(HtmlNode.DisposeClosingKoComment(), null);
        }

        public ForEachBuilder<TModel> ForEach(HtmlNode htmlNodeType, Action<StringReturningBuilder<TModel>> builder)
        {
            var nodeBuilder = new NodeBuilder(htmlNodeType);
            var element = new StringReturningBuilder<TModel>(this, nodeBuilder);
            
            if(builder != null)
                builder(element);

            var foreachDataProp = ViewModelPropertyName;
            if (string.IsNullOrWhiteSpace(foreachDataProp))
                foreachDataProp = "$root";

            var foreachAs = SanitizeAs(foreachDataProp) + "_Singular";
            var foreachArgs = string.Format("{{ data: {0}, as: '{1}' }}", foreachDataProp, foreachAs);
            element.DataBind(db => db.AddBindingNoPrefix("foreach", foreachArgs));

            var builderBase = new BuilderBase<TModel>(WebPage, foreachAs);
            return new ForEachBuilder<TModel>(builderBase, nodeBuilder);
        }

        public static string SanitizeAs(string asCandidate)
        {
            return Regex.Replace(asCandidate, "[^a-zA-Z0-9]", "");
        }
    }
}

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

        /// <summary>
        /// With a C# using block emit a ko comment foreach loop bound the the root of this builder
        /// <para>The resulting builder of this call will be scoped to 'someArray_Singular'</para>
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> @using (var arrayItem = foodGroups.ForEachKoComment())</para>
        /// <para> {</para>
        /// <para>&#160; @arrayItem.BoundTextBoxFor(x => x.Name)</para>
        /// <para> }</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;!-- ko foreach: {data:someArray,as:'someArray_Singular'} --&gt;</para>
        /// <para> &#160;&lt;input type="text" id="Name" data-bind="value: someArray_Singular.Name" /&gt;</para>
        /// <para> &lt;!-- /ko --&gt;</para> 
        /// </summary>
        /// <returns></returns>
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

            foreachDataProp = "_" + foreachDataProp + "_";

            var foreachAs = SanitizeAs(foreachDataProp) + "_Singular";
            element.DataBind(db => db.AddBindingWithJsonValue("foreach", new { _data_ = foreachDataProp, _as_ = foreachAs }));

            var builderBase = new BuilderBase<TModel>(WebPage, foreachAs);
            return new ForEachBuilder<TModel>(builderBase, nodeBuilder);
        }

        private static string SanitizeAs(string asCandidate)
        {
            return Regex.Replace(asCandidate, "[^a-zA-Z0-9]", "");
        }
    }
}

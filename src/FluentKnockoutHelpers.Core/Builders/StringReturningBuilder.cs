using System;
using System.Web;
using FluentKnockoutHelpers.Core.AttributeBuilding;

namespace FluentKnockoutHelpers.Core.Builders
{
    public class StringReturningBuilder<TModel> : Builder<TModel>, IHtmlString
    {
        internal AttributeBuilder AttributeBuilder;

        public StringReturningBuilder(BuilderBase<TModel> builder) : base(builder)
        {
        }

        public StringReturningBuilder(BuilderBase<TModel> builder, AttributeBuilder attributeBuilder)
            : base(builder)
        {
            AttributeBuilder = attributeBuilder;
        }

        /// <summary>
        /// Append a class on this builder
        /// <para>&#160;</para>
        /// <para>Example:</para>
        /// <para> &lt;input class="<paramref name="class"/>" /&gt; </para>
        /// <para>&#160;</para>
        /// </summary>
        /// <param name="class">The CSS class to append</param>
        /// <returns></returns>
        public StringReturningBuilder<TModel> Class(string @class)
        {
            EnsureAttributeBuilder();
            AttributeBuilder.Attr("class", @class);
            return this;
        }


        /// <summary>
        /// Append an inline style on this builder. Same as <see cref="Style"/>
        /// <para>&#160;</para>
        /// <para>Example:</para>
        /// <para> &lt;input style="<paramref name="styleProperty"/>: <paramref name="styleValue"/>;" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <param name="styleProperty">The css style property</param>
        /// <param name="styleValue">The css style value</param>
        /// <returns></returns>
        public StringReturningBuilder<TModel> Css(string styleProperty, string styleValue)
        {
            Attr("style", styleProperty, styleValue);
            return this;
        }

        /// <summary>
        /// Append an inline style on this builder. Same as <see cref="Css"/>
        /// <para>&#160;</para>
        /// <para>Example:</para>
        /// <para> &lt;input style="<paramref name="styleProperty"/>: <paramref name="styleValue"/>;" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <param name="styleProperty">The css style property</param>
        /// <param name="styleValue">The css style value</param>
        /// <returns></returns>
        public StringReturningBuilder<TModel> Style(string styleProperty, string styleValue)
        {
            return Css(styleProperty, styleValue);
        }

        /// <summary>
        /// Append a custom attribute on this builder. Multiple calls will result in multiple space separated values in the attribute.
        /// <para>&#160;</para>
        /// <para>Example:</para>
        /// <para> &lt;input <paramref name="attrKey"/>="<paramref name="attrValue"/>" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <param name="attrKey">The name of the attribute</param>
        /// <param name="attrValue">The attribute value to append</param>
        /// <returns></returns>
        public StringReturningBuilder<TModel> Attr(string attrKey, string attrValue)
        {
            Ensure.NotNullEmptyOrWhiteSpace(attrKey, "attrKey");
            Ensure.NotNullEmptyOrWhiteSpace(attrValue, "attrValue");

            EnsureAttributeBuilder();
            AttributeBuilder.Attr(attrKey, attrValue);

            return this;
        }

        /// <summary>
        /// Append a custom attribute on this builder. Multiple calls will result in multiple ',' separated key-value pairs in the attribute
        /// <para>&#160;</para>
        /// <para>Example:</para>
        /// <para> &lt;input <paramref name="attrKey"/>="<paramref name="innerKey"/>: <paramref name="innerValue"/>, ..." /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <param name="attrKey">The name of the attribute</param>
        /// <param name="innerKey">The attribute innerKey to append</param>
        /// <param name="innerValue">The attribute innerValue to append</param>
        /// <returns></returns>
        public StringReturningBuilder<TModel> Attr(string attrKey, string innerKey, string innerValue)
        {
            EnsureAttributeBuilder();

            AttributeBuilder.Attr(attrKey, innerKey, innerValue);

            return this;
        }

        /// <summary>
        /// Apply an id to this builder. Multiple calls will result in a run-time exception.
        /// <para>&#160;</para>
        /// <para>Example:</para>
        /// <para> &lt;input id="<paramref name="id"/>" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <param name="id">The id to apply</param>
        /// <returns></returns>
        public StringReturningBuilder<TModel> Id(string id)
        {
            EnsureAttributeBuilder();

            AttributeBuilder.Attr("id", id);

            return this;
        }

        /// <summary>
        /// Apply an name to this builder.
        /// <para>&#160;</para>
        /// <para>Example:</para>
        /// <para> &lt;input name="<paramref name="name"/>" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <param name="name">The id to apply</param>
        /// <returns></returns>
        public StringReturningBuilder<TModel> Name(string name)
        {
            EnsureAttributeBuilder();

            AttributeBuilder.Attr("name", name);

            return this;
        }

        /// <summary>
        /// Start building a Knockout data-bind on this builder
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para>&lt;input @helper.DataBind(db => db.Value(x => x.Name)) /&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input data-bind="value: Name" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <param name="builder">The id to apply</param>
        /// <returns></returns>
        public override StringReturningBuilder<TModel> DataBind(Action<DataBindBuilder<TModel>> builder)
        {
            builder(new DataBindBuilder<TModel>(this));
            return this;
        }

        /// <summary>
        /// Create an AttributeBuilder if it doesn't already exist to start writing attributes to it
        /// </summary>
        protected void EnsureAttributeBuilder()
        {
            if (AttributeBuilder == null)
                AttributeBuilder = new AttributeBuilder();
        }

        /// <summary>
        /// Write out the current contents of the AttributeBuilder
        /// </summary>
        /// <returns></returns>
        protected string FlushAttributeBuilder()
        {
            var result = AttributeBuilder.GetContents();
            AttributeBuilder = null;
            return result;
        }

        /// <summary>
        /// Write out the current contents of this builder
        /// </summary>
        /// <returns></returns>
        public string ToHtmlString()
        {
            return FlushAttributeBuilder();
        }
    }
}

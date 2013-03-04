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

        public StringReturningBuilder<TModel> Class(string @class)
        {
            EnsureAttributeBuilder();
            AttributeBuilder.Attr("class", @class);
            return this;
        }

        public StringReturningBuilder<TModel> Css(string styleProperty, string styleValue)
        {
            Attr("style", styleProperty, styleValue);
            return this;
        }

        public StringReturningBuilder<TModel> Style(string styleProperty, string styleValue)
        {
            return Css(styleProperty, styleValue);
        }

        public StringReturningBuilder<TModel> Attr(string attrKey, string attrValue)
        {
            Ensure.NotNullEmptyOrWhiteSpace(attrKey, "attrKey");
            Ensure.NotNullEmptyOrWhiteSpace(attrValue, "attrValue");

            EnsureAttributeBuilder();
            AttributeBuilder.Attr(attrKey, attrValue);

            return this;
        }

        public StringReturningBuilder<TModel> Attr(string attrKey, string innerKey, string innerValue)
        {
            EnsureAttributeBuilder();

            AttributeBuilder.Attr(attrKey, innerKey, innerValue);

            return this;
        }

        public StringReturningBuilder<TModel> Id(string id)
        {
            EnsureAttributeBuilder();

            AttributeBuilder.Attr("id", id);

            return this;
        }

        public override StringReturningBuilder<TModel> DataBind(Action<DataBindBuilder<TModel>> builder)
        {
            builder(new DataBindBuilder<TModel>(this));
            return this;
        }

        protected void EnsureAttributeBuilder()
        {
            if (AttributeBuilder == null)
                AttributeBuilder = new AttributeBuilder();
        }

        protected string FlushAttributeBuilder()
        {
            var result = AttributeBuilder.GetContents();
            AttributeBuilder = null;
            return result;
        }

        public string ToHtmlString()
        {
            return FlushAttributeBuilder();
        }
    }
}

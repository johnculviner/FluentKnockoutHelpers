using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using FluentKnockoutHelpers.Core.AttributeBuilding;
using FluentKnockoutHelpers.Core.NodeBuilding;
using FluentKnockoutHelpers.Core.Utility;

namespace FluentKnockoutHelpers.Core.Builders
{
    public class Builder<TModel>
    {
        internal readonly string ViewModelPropertyName;
        internal AttributeBuilder AttributeBuilder;
        internal IHtmlHelperAdapter HtmlHelper;

        public Builder(IHtmlHelperAdapter htmlHelper)
            : this(htmlHelper, null)
        {
        }

        public Builder(IHtmlHelperAdapter htmlHelper, string viewModelPropertyName)
        {
            HtmlHelper = htmlHelper;
            ViewModelPropertyName = viewModelPropertyName;
        }

        protected Builder(Builder<TModel> builder)
        {
            ViewModelPropertyName = builder.ViewModelPropertyName;
            AttributeBuilder = builder.AttributeBuilder;
            HtmlHelper = builder.HtmlHelper;
        }

        public DisposableBuilder<TModel> Element(string tag, Action<StringReturningBuilder<TModel>> builder)
        {
            AttributeBuilder = new NodeBuilder(Node.Element(tag, TagClosingMode.OnDispose));
            var element = new StringReturningBuilder<TModel>(this);
            builder(element);
            ImmediatelyWriteToResponse(element.ToHtmlString());
            return new DisposableBuilder<TModel>(this);
        }

        public StringReturningBuilder<TModel> SelfClosingElement(string tag)
        {
            AttributeBuilder = new NodeBuilder(Node.Element(tag, TagClosingMode.Self));
            return new StringReturningBuilder<TModel>(this);
        }

        public StringReturningBuilder<TModel> DataBind(Action<DataBindBuilder<TModel>> builder)
        {
            var stringBuilder = new StringReturningBuilder<TModel>(this);
            builder(new DataBindBuilder<TModel>(stringBuilder));
            return stringBuilder;
        }

        public ForEachBuilder<TModel, TInner> ForEach<TInner>(Node node, Action<StringReturningBuilder<TModel>> builder)
        {
            AttributeBuilder = new NodeBuilder(node);
            var element = new StringReturningBuilder<TModel>(this);
            builder(element);
            ImmediatelyWriteToResponse(element.ToHtmlString());
            return new ForEachBuilder<TModel, TInner>(this);
        }


        public IHtmlString DisplayNameFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return new HtmlString(ExpressionParser.DisplayNameFor(propExpr));
        }

        protected void ImmediatelyWriteToResponse(string s)
        {
            HtmlHelper.WriteToOutput(s);
        }

        public IHtmlString AssignObservableFor<TProp>(Expression<Func<TModel, TProp>> propExpr, string arg)
        {
            return new HtmlString(string.Format("{0}({1})", EmitPropTextFor(propExpr), arg));
        }

        public IHtmlString EvalObservableFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return new HtmlString(string.Format("{0}()", EmitPropTextFor(propExpr)));
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //TODO remove dependency on MVC
        public IHtmlString EmitPropTextFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return string.IsNullOrWhiteSpace(ViewModelPropertyName) ?
                new HtmlString(ExpressionHelper.GetExpressionText(propExpr)) :
                new HtmlString(string.Format("{0}.{1}", ViewModelPropertyName, ExpressionHelper.GetExpressionText(propExpr)));
        }
    }
}

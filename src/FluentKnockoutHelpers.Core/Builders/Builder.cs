using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using FluentKnockoutHelpers.Core.NodeBuilding;
using FluentKnockoutHelpers.Core.Utility;

namespace FluentKnockoutHelpers.Core.Builders
{
    public class Builder<TModel>
    {
        public readonly string ViewModelPropertyName;
        public WebPageBase WebPage;

        public Builder(WebPageBase webPage)
            : this(webPage, null)
        {
        }

        public Builder(WebPageBase webPage, string viewModelPropertyName)
        {
            WebPage = webPage;
            ViewModelPropertyName = viewModelPropertyName;
        }

        protected Builder(Builder<TModel> builder)
        {
            ViewModelPropertyName = builder.ViewModelPropertyName;
            WebPage = builder.WebPage;
        }

        public virtual DisposableBuilder<TModel> Element(string tag, Action<StringReturningBuilder<TModel>> builder)
        {
            var nodeBuilder = new NodeBuilder(new DisposeClosingElement(tag));
            var element = new StringReturningBuilder<TModel>(this, nodeBuilder);
            builder(element);
            return new DisposableBuilder<TModel>(this, nodeBuilder);
        }

        public virtual StringReturningBuilder<TModel> SelfClosingElement(string tag)
        {
            return new StringReturningBuilder<TModel>(this, new NodeBuilder(new SelfClosingElement(tag)));
        }

        public virtual StringReturningBuilder<TModel> SelfClosingElement(string tag, string innerHtml)
        {
            return new StringReturningBuilder<TModel>(this, new NodeBuilder(new SelfClosingElement(tag, innerHtml)));
        }

        public virtual StringReturningBuilder<TModel> DataBind(Action<DataBindBuilder<TModel>> builder)
        {
            var stringBuilder = new StringReturningBuilder<TModel>(this);
            builder(new DataBindBuilder<TModel>(stringBuilder));
            return stringBuilder;
        }

        public virtual StringReturningBuilder<TModel> LabelFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return SelfClosingElement("label", DisplayNameFor(propExpr).ToString()).Attr("for", ExpressionParser.GetExpressionText(propExpr));
        }

        public virtual StringReturningBuilder<TModel> BoundTextBoxFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            var exprText = ExpressionParser.GetExpressionText(propExpr); //avoid 2x expr parsing
            //TODO: Multiple databinds broken!
            return SelfClosingElement("input").Attr("type", "text").Id(exprText).DataBind(db => db.Value(exprText).ValueUpdate(ValueUpdate.KeyUp));
        }

        public virtual IHtmlString DisplayNameFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return new HtmlString(ExpressionParser.DisplayNameFor(propExpr));
        }

        public virtual IHtmlString AssignObservableFor<TProp>(Expression<Func<TModel, TProp>> propExpr, string arg)
        {
            return new HtmlString(string.Format("{0}({1})", EmitPropTextFor(propExpr), arg));
        }

        public virtual IHtmlString EvalObservableFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return new HtmlString(string.Format("{0}()", EmitPropTextFor(propExpr)));
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public virtual IHtmlString EmitPropTextFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return string.IsNullOrWhiteSpace(ViewModelPropertyName) ?
                new HtmlString(ExpressionParser.GetExpressionText(propExpr)) :
                new HtmlString(string.Format("{0}.{1}", ViewModelPropertyName, ExpressionHelper.GetExpressionText(propExpr)));
        }


        protected void ImmediatelyWriteToResponse(string s)
        {
            WebPage.WriteLiteral(s);
        }


        //public ForEachBuilder<TModel, TInner> ForEach<TInner>(Node node, Action<StringReturningBuilder<TModel>> builder)
        //{
        //    var element = new StringReturningBuilder<TModel>(this, new NodeBuilder(node));
        //    builder(element);
        //    ImmediatelyWriteToResponse(element.ToHtmlString());
        //    return new ForEachBuilder<TModel, TInner>(this);
        //}


        //Ugly way, but works
        //public IHtmlString LabelFor<TProp>(Expression<Func<TModel, TProp>> propExpr, Action<StringReturningBuilder<TModel>> builder)
        //{
        //    using (var label = Element("label", b =>
        //        {
        //            b.Attr("for", ExpressionParser.GetExpressionText(propExpr));
        //            builder(b); //tack on add'l provided attributes
        //        }))
        //    {
        //        label.WebPage.WriteLiteral(DisplayNameFor(propExpr).ToString());
        //    }
        //    return null; //all writing is done directly against the page
        //}
    }
}

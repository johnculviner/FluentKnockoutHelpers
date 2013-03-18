using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using FluentKnockoutHelpers.Core.NodeBuilding;
using FluentKnockoutHelpers.Core.Utility;

namespace FluentKnockoutHelpers.Core.Builders
{
    /// <summary>
    /// A builder for a generic type
    /// </summary>
    /// <typeparam name="TModel">The type of model to create a builder for</typeparam>
    public class Builder<TModel> : BuilderBase<TModel>
    {
        #region ctor
        public Builder(WebPageBase webPage)
            : base(webPage)
        {
        }

        public Builder(WebPageBase webPage, string viewModelPropertyName)
            : base(webPage, viewModelPropertyName)
        {
        }

        protected Builder(BuilderBase<TModel> builder)
            : base(builder)
        {
        }
        #endregion

        #region HtmlNode Building

        public virtual DisposableBuilder<TModel> ElementUsing(string elementTag, Action<StringReturningBuilder<TModel>> builder)
        {
            var nodeBuilder = new NodeBuilder(new DisposeClosingHtmlElement(elementTag));
            var element = new StringReturningBuilder<TModel>(this, nodeBuilder);
            builder(element);
            return new DisposableBuilder<TModel>(this, nodeBuilder);
        }

        /// <summary>
        /// Begin building a self closing element for the given 'elementTag'
        /// </summary>
        /// <param name="elementTag"></param>
        /// <returns></returns>
        public virtual StringReturningBuilder<TModel> ElementSelfClosing(string elementTag)
        {
            return new StringReturningBuilder<TModel>(this, new NodeBuilder(new SelfClosingHtmlElement(elementTag)));
        }

        /// <summary>
        /// Begin building a self closing element for the given 'elementTag' with the specified 'innerHtml'
        /// </summary>
        /// <param name="elementTag"></param>
        /// <param name="innerHtml"></param>
        /// <returns></returns>
        public virtual StringReturningBuilder<TModel> ElementSelfClosing(string elementTag, string innerHtml)
        {
            return new StringReturningBuilder<TModel>(this, new NodeBuilder(new SelfClosingHtmlElement(elementTag, innerHtml)));
        }

        public virtual StringReturningBuilder<TModel> KoCommentSelfClosing()
        {
            return new StringReturningBuilder<TModel>(this, new NodeBuilder(HtmlNode.SelfClosingKoComment()));
        }

        public virtual IHtmlString KoCommentDisposable()
        {
            throw new NotImplementedException();
        }
        #endregion


        public virtual StringReturningBuilder<TModel> DataBind(Action<DataBindBuilder<TModel>> builder)
        {
            var stringBuilder = new StringReturningBuilder<TModel>(this);
            builder(new DataBindBuilder<TModel>(stringBuilder));
            return stringBuilder;
        }

        #region Direct Evaluation & Assignment
        public virtual IHtmlString AssignObservableFor<TProp>(Expression<Func<TModel, TProp>> propExpr, string arg)
        {
            return new HtmlString(string.Format("{0}({1})", PropStringFor(propExpr), arg));
        }

        public virtual IHtmlString EvalObservableFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return new HtmlString(string.Format("{0}()", PropStringFor(propExpr)));
        }

        public virtual string PropStringFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return string.IsNullOrWhiteSpace(ViewModelPropertyName) ?
                ExpressionParser.GetExpressionText(propExpr) :
                string.Format("{0}.{1}", ViewModelPropertyName, ExpressionParser.GetExpressionText(propExpr));
        }

        public virtual IHtmlString GetExpressionTextFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return new HtmlString(ExpressionParser.GetExpressionText(propExpr));
        }
        #endregion

        /// <summary>
        /// Begin building a &lt;label for="{{passed 'propExpr'}}"&gt; {{Display attribute or property name of 'propExpr'}} &lt;/label&gt;.
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public virtual StringReturningBuilder<TModel> LabelFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return ElementSelfClosing("label", DisplayNameFor(propExpr).ToString()).Attr("for", ExpressionParser.GetExpressionText(propExpr));
        }



        #region Bound{{...}}For
        /// <summary>
        /// Begin building a &lt;input type=text .../&gt; bound to the passed 'propExpr' via Knockout
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public virtual StringReturningBuilder<TModel> BoundTextBoxFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            var exprText = ExpressionParser.GetExpressionText(propExpr); //avoid 2x expr parsing
            return ElementSelfClosing("input").Attr("type", "text").Id(exprText).DataBind(db => db.Value(exprText).ValueUpdate(ValueUpdate.KeyUp));
        }

        /// <summary>
        /// Emits a ko comment block (no element) with it's text bound to the 'propExpr' via Knockout
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public virtual IHtmlString BoundTextFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return KoCommentSelfClosing().DataBind(db => db.Text(propExpr));
        }

        #endregion


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

using System;
using System.Collections;
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

        /// <summary>
        /// Emit
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propExpr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual IHtmlString AssignObservableFor<TProp>(Expression<Func<TModel, TProp>> propExpr, string value)
        {
            return new HtmlString(string.Format("{0}({1})", PropStringFor(propExpr), value));
        }

        /// <summary>
        /// Emit 
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public virtual IHtmlString EvalObservableFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return new HtmlString(string.Format("{0}()", PropStringFor(propExpr)));
        }

        /// <summary>
        /// Emit a string for for a particular view model property. Can be used when a pre-build binding doesn't exist.
        /// <para>&#160;</para>
        /// <para>Assuming:</para>
        /// <para> var surveyHelper = this.KnockoutHelperForType&lt;Survey&gt;("survey", false);</para>
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> &lt;input type="text" data-bind="datepicker: @surveyHelper.PropStringFor(x =&gt; x.DateOfBirth)" /&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input type="text" data-bind="datepicker: survey.DateOfBirth" /&gt;</para>
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public virtual string PropStringFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return string.IsNullOrWhiteSpace(ViewModelPropertyName) ?
                ExpressionParser.GetExpressionText(propExpr) :
                string.Format("{0}.{1}", ViewModelPropertyName, ExpressionParser.GetExpressionText(propExpr));
        }

        /// <summary>
        /// Emit the expression text for a view model property
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> &lt;label for="@helper.ExpressionTextFor(x => x.FirstName)" &gt;First Name&lt;label&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;label for="FirstName" &gt;First Name&lt;label&gt;</para>
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public virtual IHtmlString GetExpressionTextFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return new HtmlString(ExpressionParser.GetExpressionText(propExpr));
        }
        #endregion

        /// <summary>
        /// Start building a label for a particular view model property. The label's text will be derived from any [Display(..)] DataAnnotation
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> @helper.LabelFor(x => x.FirstName)</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;label for="FirstName" &gt;First Name&lt;/label&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public virtual StringReturningBuilder<TModel> LabelFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return ElementSelfClosing("label", DisplayNameFor(propExpr).ToString()).Attr("for", ExpressionParser.GetExpressionText(propExpr));
        }



        #region Bound ___ For
        
        /// <summary>
        /// Start building a text box bound with knockout to a particular view model property.
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> @helper.BoundTextBoxFor(x => x.FirstName)</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input type="text" id="FirstName" data-bind="value: FirstName" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public virtual StringReturningBuilder<TModel> BoundTextBoxFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            var exprText = ExpressionParser.GetExpressionText(propExpr); //avoid 2x expr parsing
            return ElementSelfClosing("input").Attr("type", "text").Id(exprText).DataBind(db => db.Value(exprText));
        }

        /// <summary>
        /// Emits a Knockout Comment block (a 'virtual element') bound to a particular view model property
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> @helper.BoundTextFor(x => x.FirstName)</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;!-- ko text: FirstName --&gt; *VALUE* &lt;!-- /ko --&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public virtual IHtmlString BoundTextFor<TProp>(Expression<Func<TModel, TProp>> propExpr)
        {
            return KoCommentSelfClosing().DataBind(db => db.Text(propExpr));
        }

        /// <summary>
        /// Start building an checkbox bound with knockout to a particular *BOOLEAN* view model property
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> @helper.BoundCheckBoxFor(x => x.IsCool)</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input type="checkbox" id="IsCool" data-bind="value: IsCool" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <param name="propExpr"></param>
        /// <returns></returns>
        public virtual StringReturningBuilder<TModel> BoundCheckBoxFor(Expression<Func<TModel, bool>> propExpr)
        {
            var exprText = ExpressionParser.GetExpressionText(propExpr); //avoid 2x expr parsing
            return ElementSelfClosing("input").Attr("type", "checkbox").Id(exprText).DataBind(db => db.Checked(propExpr));
        }


        /// <summary>
        /// Bind a checkbox to an *ARRAY* view model property bound to the specified value being in the array
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> @helper.BoundCheckBoxFor(x => x.States, "MN")</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input type="checkbox" value="MN" data-bind="checked: States"  /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="propExpr"> </param>
        /// <param name="value"> </param>
        /// <returns></returns>
        public virtual StringReturningBuilder<TModel> BoundCheckBoxFor(Expression<Func<TModel, IEnumerable>> propExpr, object value)
        {
            var exprText = ExpressionParser.GetExpressionText(propExpr); //avoid 2x expr parsing
            return ElementSelfClosing("input").Attr("type", "checkbox").Id(exprText).DataBind(db => db.Checked(propExpr, value));
        }

        /// <summary>
        /// Bind a radio button to a particular view model property. The radio button name will be the property name unless overridden.
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> @helper.BoundRadioButtonFor(x => x.Gender, Gender.Male)</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input type="radio" name="Gender" value="Male" data-bind="checked: Gender" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProp"> </typeparam>
        /// <param name="propExpr"> </param>
        /// <param name="value"> </param>
        /// <param name="name"> </param>
        /// <returns></returns>
        public virtual StringReturningBuilder<TModel> BoundRadioButtonFor<TProp>(Expression<Func<TModel, TProp>> propExpr, object value, string name = null)
        {
            var exprText = ExpressionParser.GetExpressionText(propExpr); //avoid 2x expr parsing
            return ElementSelfClosing("input")
                    .Attr("type", "radio")
                    .Name(name ?? exprText)
                    .DataBind(db => db.Checked(propExpr, value));
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

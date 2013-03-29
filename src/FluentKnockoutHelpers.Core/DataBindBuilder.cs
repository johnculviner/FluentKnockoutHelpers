using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentKnockoutHelpers.Core.Builders;
using FluentKnockoutHelpers.Core.Utility;

namespace FluentKnockoutHelpers.Core
{
    public class DataBindBuilder<TModel>
    {
        public StringReturningBuilder<TModel> Builder;

        public DataBindBuilder(StringReturningBuilder<TModel> builder)
        {
            Builder = builder;
        }

        public DataBindBuilder<TModel> Custom<TProp>(string bindingName, Expression<Func<TModel, TProp>> text)
        {
            return Custom(bindingName, ExpressionParser.GetExpressionText(text));
        }

        public DataBindBuilder<TModel> Custom(string bindingName, string bindingProperty)
        {
            Builder.Attr("data-bind", bindingName, GetDataBindPropertyName(bindingProperty));
            return this;
        }

        /// <summary>
        /// Create a 'data-bind' binding without prefixing the viewModelPropertyName
        /// </summary>
        /// <param name="bindingName"></param>
        /// <param name="bindingProperty"></param>
        /// <returns></returns>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public DataBindBuilder<TModel> AddBindingNoPrefix(string bindingName, string bindingProperty)
        {
            Builder.Attr("data-bind", bindingName, bindingProperty);
            return this;
        }

        /// <summary>
        /// Create a data-bind binding with an anonymous type that will be converted to JSON
        /// </summary>
        /// <param name="bindingName">"data-bind" binding name</param>
        /// <param name="anonymousType">Object that will be converted to JSON</param>
        /// <returns></returns>
        /// <example>
        /// AddBindingWithJsonValue("foo", new { bar : "baz" })   ==   data-bind="foo: {'bar' : 'baz'}
        /// OR
        /// AddBindingWithJsonValue("foo", new { bar : 42 })   ==   data-bind="foo: {'bar' : 42}
        /// OR
        /// AddBindingWithJsonValue("foo", new { _bar_ : "baz" })   ==   data-bind="foo: { bar : 'baz'}
        /// </example>
        public DataBindBuilder<TModel> AddBindingWithJsonValue(string bindingName, object anonymousType)
        {
            AddBindingNoPrefix(bindingName, ParseBindingJson(anonymousType));
            return this;
        }

        private static string ParseBindingJson(object anonymousType)
        {
            var result = GlobalSettings.JsonSerializer.ToJsonString(anonymousType);

            //parse out "_foo_" to be foo
            result = result.Replace("\"_", "").Replace("_\"", "");

            //parse out " to be ' because we are working inside an attribute where " will escape
            result = result.Replace("\"", "'");

            return result;
        }

        private string GetDataBindPropertyName(string propName)
        {
            if (string.IsNullOrEmpty(Builder.ViewModelPropertyName))
                return propName;

            return Builder.ViewModelPropertyName + "." + propName;
        }
    }

    public static class DataBindBuilderExtensions
    {
        #region Text
        //credit: http://knockoutjs.com/documentation/text-binding.html
        /// <summary>
        /// Binds the textual contents of an element to the value of the specified view model property
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> &lt;div @helper.DataBind(db => db.Value(x => x.Name))&gt;&lt;/div&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;div data-bind="value: Name"&gt;{{Name's current value}}&lt;/div&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="this"></param>
        /// <param name="bindingProperty"></param>
        /// <returns></returns>
        public static DataBindBuilder<TModel> Text<TModel, TValue>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, TValue>> bindingProperty)
        {
            return @this.Custom("text", bindingProperty);
        }


        //credit: http://knockoutjs.com/documentation/text-binding.html
        /// <summary>
        /// Binds the textual contents of an element to the value of the specified view model property
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> &lt;div @helper.DataBind(db => db.Value("Name"))&gt;&lt;/div&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;div data-bind="value: Name"&gt;{{Name's current value}}&lt;/div&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="this"></param>
        /// <param name="bindingProperty"></param>
        /// <returns></returns>
        public static DataBindBuilder<TModel> Text<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.Custom("text", bindingProperty);
        }
        #endregion

        #region Value
        //credit: http://knockoutjs.com/documentation/value-binding.html
        /// <summary>
        /// The value binding links the associated DOM element’s value with a property on your view model.
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para>&lt;input @helper.DataBind(db => db.Value(x => x.Name)) /&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input data-bind="value: Name" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="this"></param>
        /// <param name="bindingProperty">A lamda, property expression referring to the view model property to bind to</param>
        /// <returns></returns>
        public static DataBindBuilder<TModel> Value<TModel, TValue>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, TValue>> bindingProperty)
        {
            return @this.Custom("value", bindingProperty);
        }

        //credit: http://knockoutjs.com/documentation/value-binding.html
        /// <summary>
        /// The value binding links the associated DOM element’s value with a property on your view model.
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para>&lt;input @helper.DataBind(db => db.Value("Name")) /&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input data-bind="value: Name" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="this"></param>
        /// <param name="bindingProperty">A lamda, property expression referring to the view model property to bind to</param>
        /// <returns></returns>
        public static DataBindBuilder<TModel> Value<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.Custom("value", bindingProperty);
        }
        #endregion

        #region If
        public static DataBindBuilder<TModel> If<TModel, TValue>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, TValue>> bindingProperty)
        {
            return @this.Custom("if", bindingProperty);
        }

        public static DataBindBuilder<TModel> If<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.Custom("if", bindingProperty);
        }
        #endregion

        #region Visible
        public static DataBindBuilder<TModel> Visible<TModel>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, bool>> bindingProperty)
        {
            return @this.Custom("visible", bindingProperty);
        }

        public static DataBindBuilder<TModel> Visible<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.Custom("visible", bindingProperty);
        }
        #endregion

        #region Checked

        /// <summary>
        /// Bind a checkbox to a *boolean* view model property
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> &lt;input type="checkbox" @helper.DataBind(db => db.Checked(x => x.IsCool)) /&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input type="checkbox" data-bind="checked: IsCool" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="this"></param>
        /// <param name="bindingProperty"></param>
        /// <returns></returns>
        public static DataBindBuilder<TModel> Checked<TModel>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, bool>> bindingProperty)
        {
            return @this.Custom("checked", bindingProperty);
        }

        /// <summary>
        /// Bind a checkbox to a boolean view model property
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> &lt;input type="checkbox" @helper.DataBind(db => db.Checked("IsCool")) /&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input type="checkbox" data-bind="checked: isCool" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="this"></param>
        /// <param name="bindingProperty"></param>
        /// <returns></returns>
        public static DataBindBuilder<TModel> Checked<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.Custom("checked", bindingProperty);
        }

        /// <summary>
        /// Bind a checkbox to an *ARRAY* view model property bound to the specified value being in the array
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> &lt;input type="checkbox" @helper.DataBind(db => db.Checked(x => x.States, "MN")) /&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input type="checkbox" value="MN" data-bind="checked: States"  /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="this"></param>
        /// <param name="bindingProperty"></param>
        /// <param name="value"> </param>
        /// <returns></returns>
        public static DataBindBuilder<TModel> Checked<TModel>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, IEnumerable>> bindingProperty, object value)
        {
            @this.Builder.Attr("value", value.ToString());
            return @this.Custom("checked", bindingProperty);
        }

        /// <summary>
        /// Bind a radio button to a particular view model property
        /// <para>&#160;</para>
        /// <para>Usage Example:</para>
        /// <para> &lt;input type="radio" @helper.DataBind(db => db.Checked(x => x.Gender, Gender.Male)) /&gt;</para>
        /// <para>&#160;</para>
        /// <para>Result:</para>
        /// <para> &lt;input type="radio" value="Male" data-bind="checked: Gender"  /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"> </typeparam>
        /// <param name="this"></param>
        /// <param name="bindingProperty"></param>
        /// <param name="value"> </param>
        /// <returns></returns>
        public static DataBindBuilder<TModel> Checked<TModel, TValue>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, TValue>> bindingProperty, object value)
        {
            @this.Builder.Attr("value", value.ToString());
            return @this.Custom("checked", bindingProperty);
        }

        #endregion

        /// <summary>
        /// When combined with a "Value" bind, controls when the view model is updated
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="this"></param>
        /// <param name="valueUpdate"></param>
        /// <returns></returns>
        public static DataBindBuilder<TModel> ValueUpdate<TModel>(this DataBindBuilder<TModel> @this, ValueUpdate valueUpdate)
        {
            @this.Builder.Attr("data-bind", "valueUpdate", string.Format("'{0}'", valueUpdate.ToString().ToLowerInvariant()));
            return @this;
        }

        public static DataBindBuilder<TModel> Click<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.Custom("click", bindingProperty);
        }
    }

    public enum ValueUpdate
    {
        //credit: http://knockoutjs.com/documentation/value-binding.html
        /// <summary>
        /// Updates your view model when the user releases a key
        /// </summary>
        KeyUp,

        //credit: http://knockoutjs.com/documentation/value-binding.html
        /// <summary>
        /// Updates your view model when the user has typed a key. Unlike keyup, this updates repeatedly while the user holds a key down
        /// </summary>
        KeyPress,

        //credit: http://knockoutjs.com/documentation/value-binding.html
        /// <summary>
        /// Updates your view model as soon as the user begins typing a character. This works by catching the browser’s keydown event and handling the event asynchronously.
        /// </summary>
        AfterKeyDown
    }
}
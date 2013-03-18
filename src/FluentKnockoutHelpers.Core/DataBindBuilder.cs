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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public DataBindBuilder<TModel> AddBinding<TProp>(string bindingName, Expression<Func<TModel, TProp>> text)
        {
            return AddBinding(bindingName, ExpressionParser.GetExpressionText(text));
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public DataBindBuilder<TModel> AddBinding(string bindingName, string bindingProperty)
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
        public static DataBindBuilder<TModel> Text<TModel, TValue>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, TValue>> bindingProperty)
        {
            return @this.AddBinding("text", bindingProperty);
        }

        public static DataBindBuilder<TModel> Text<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.AddBinding("text", bindingProperty);
        }
        #endregion

        #region Value
        public static DataBindBuilder<TModel> Value<TModel, TValue>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, TValue>> bindingProperty)
        {
            return @this.AddBinding("value", bindingProperty);
        }

        public static DataBindBuilder<TModel> Value<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.AddBinding("value", bindingProperty);
        }
        #endregion

        #region If
        public static DataBindBuilder<TModel> If<TModel, TValue>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, TValue>> bindingProperty)
        {
            return @this.AddBinding("if", bindingProperty);
        }

        public static DataBindBuilder<TModel> If<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.AddBinding("if", bindingProperty);
        }
        #endregion

        #region Visible
        public static DataBindBuilder<TModel> Visible<TModel>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, bool>> bindingProperty)
        {
            return @this.AddBinding("visible", bindingProperty);
        }

        public static DataBindBuilder<TModel> Visible<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.AddBinding("visible", bindingProperty);
        }
        #endregion

        #region Checked
        public static DataBindBuilder<TModel> Checked<TModel>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, bool>> bindingProperty)
        {
            return @this.AddBinding("checked", bindingProperty);
        }

        public static DataBindBuilder<TModel> Checked<TModel>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, IEnumerable>> bindingProperty)
        {
            return @this.AddBinding("checked", bindingProperty);
        }

        public static DataBindBuilder<TModel> Checked<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.AddBinding("checked", bindingProperty);
        }
        #endregion

        public static DataBindBuilder<TModel> ValueUpdate<TModel>(this DataBindBuilder<TModel> @this, ValueUpdate valueUpdate)
        {
            @this.Builder.Attr("data-bind", "valueUpdate", string.Format("'{0}'", valueUpdate.ToString().ToLowerInvariant()));
            return @this;
        }

        public static DataBindBuilder<TModel> Click<TModel>(this DataBindBuilder<TModel> @this, Expression<Func<TModel, bool>> bindingProperty)
        {
            return @this.AddBinding("click", bindingProperty);
        }

        public static DataBindBuilder<TModel> Click<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.AddBinding("click", bindingProperty);
        }
    }

    public enum ValueUpdate
    {
        KeyUp,
        KeyPress,
        AfterKeyDown
    }
}
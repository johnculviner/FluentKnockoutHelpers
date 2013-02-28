using System;
using System.Collections;
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

        #region Foreach

        public static DataBindBuilder<TModel> Foreach<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.AddBinding("foreach", bindingProperty);
        }
        #endregion

        public static DataBindBuilder<TModel> ValueUpdate<TModel>(this DataBindBuilder<TModel> @this, ValueUpdate valueUpdate)
        {
            @this.Builder.Attr("data-bind", "valueUpdate", string.Format("'{0}'", valueUpdate.ToString().ToLowerInvariant()));
            return @this;
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
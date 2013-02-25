using System;
using System.Collections;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentKnockoutHelpers.Core.Builders;

namespace FluentKnockoutHelpers.Core
{
    public class DataBindBuilder<TModel>
    {
        private StringReturningBuilder<TModel> _builder;

        public DataBindBuilder(StringReturningBuilder<TModel> builder)
        {
            _builder = builder;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //TODO remove dependency on MVC
        public DataBindBuilder<TModel> AddBinding<TProp>(string bindingName, Expression<Func<TModel, TProp>> text)
        {
            return AddBinding(bindingName, ExpressionHelper.GetExpressionText(text));
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public DataBindBuilder<TModel> AddBinding(string bindingName, string bindingProperty)
        {
            _builder.Attr("data-bind", bindingName, GetDataBindPropertyName(bindingProperty));
            return this;
        }

        private string GetDataBindPropertyName(string propName)
        {
            if (string.IsNullOrEmpty(_builder.ViewModelPropertyName))
                return propName;

            return _builder.ViewModelPropertyName + "." + propName;
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


        #region Click
        public static DataBindBuilder<TModel> Click<TModel>(this DataBindBuilder<TModel> @this, string bindingProperty)
        {
            return @this.AddBinding("click", bindingProperty);
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


    }
}
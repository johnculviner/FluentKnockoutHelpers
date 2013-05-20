using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FluentKnockoutHelpers.Core.Settings;

namespace FluentKnockoutHelpers.Core.Utility
{
    /// <summary>
    /// Parses Expressions
    /// </summary>
    public static class ExpressionParser
    {
        /// <summary>
        /// This will return a member expression from a lambda expression
        /// </summary>
        /// <param name="expr">the lambda expression</param>
        /// <returns></returns>
        private static MemberExpression ToMemberExpression(LambdaExpression expr)
        {
            if (expr.Body is UnaryExpression)
                return ((MemberExpression)((UnaryExpression)expr.Body).Operand);

            if (expr.Body is MemberExpression)
                return ((MemberExpression)expr.Body);

            throw new Exception(string.Format("Could not find a member expression in {0}", expr));
        }

        /// <summary>
        /// Resolve the [Display]Attribute with any localizations resolving RequiredTokenSettings
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string DisplayNameForLabel<TParameter, TValue>(Expression<Func<TParameter, TValue>> expr)
        {
            var memberExpression = ToMemberExpression(expr);
            var type = typeof(TParameter);

            if (memberExpression.Expression != null && memberExpression.Expression.NodeType != ExpressionType.Parameter)
                type = memberExpression.Expression.Type;

            var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, type);

            var propertyName = memberExpression.Member.Name;
            var propMetadata = metaData.Properties.FirstOrDefault(p => p.PropertyName == propertyName);

            if (propMetadata == null)
                return CamelCaseSpacer(propertyName);

            if (GlobalSettings.RequiredTokenSettings.IsEnabled && propMetadata.IsRequired)
            {
                if (GlobalSettings.RequiredTokenSettings.RequiredTokenPosition == RequiredTokenPosition.LeftOfLabel)
                    return GlobalSettings.RequiredTokenSettings.RequiredToken + " " + (propMetadata.DisplayName ?? CamelCaseSpacer(propMetadata.PropertyName));

                return (propMetadata.DisplayName ?? CamelCaseSpacer(propMetadata.PropertyName)) + " " + GlobalSettings.RequiredTokenSettings.RequiredToken;
            }

            return propMetadata.DisplayName ?? CamelCaseSpacer(propMetadata.PropertyName);
        }

        /// <summary>
        /// Resolve the [Display]Attribute with any localizations
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string DisplayName<TParameter, TValue>(Expression<Func<TParameter, TValue>> expr)
        {
            var memberExpression = ToMemberExpression(expr);
            var type = typeof(TParameter);

            if (memberExpression.Expression != null && memberExpression.Expression.NodeType != ExpressionType.Parameter)
                type = memberExpression.Expression.Type;

            var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, type);

            var propertyName = memberExpression.Member.Name;
            var propMetadata = metaData.Properties.FirstOrDefault(p => p.PropertyName == propertyName);

            if (propMetadata == null)
                return CamelCaseSpacer(propertyName);

            return propMetadata.DisplayName ?? CamelCaseSpacer(propMetadata.PropertyName);
        }

        //FirstName => First Name
        private static string CamelCaseSpacer(string propName)
        {
            return Regex.Replace(propName, "([A-Z])", " $1", RegexOptions.Compiled);
        }

        //TODO: support nesting in the future if needed
        public static string GetExpressionText(LambdaExpression expr)
        {
            return ToMemberExpression(expr).Member.Name;
        }
    }
}

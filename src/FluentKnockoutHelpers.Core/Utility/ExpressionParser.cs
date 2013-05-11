using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;

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
        /// Resolve the [Display]Attribute with any localizations
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        //TODO remove dependency on MVC
        public static string DisplayNameFor<TParameter, TValue>(Expression<Func<TParameter, TValue>> expr)
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

        /// <summary>
        /// Determine if the property in the Lambda Expression can be assigned a null
        /// </summary>
        /// <param name="expr">Expression containing property to check</param>
        /// <returns></returns>
        public static bool ExpressionCanBeAssignedNull(LambdaExpression expr)
        {
            var type = ToMemberExpression(expr).Type;
            return !type.IsValueType || (Nullable.GetUnderlyingType(type) != null);
        }

        /// <summary>
        /// Determine if the property in the Lambda Expression has a paramater attribute type on it.
        /// </summary>
        /// <param name="expr">Expression containing the property</param>
        /// <param name="type">The type of attribute to check for</param>
        /// <returns></returns>
        public static bool ExpressionHasAttribute(LambdaExpression expr, Type type)
        {
            /*TODO: 
             * I'm assuming that the first paramater of the expression is the Class that contains the property to check. 
             * May not be the best way of doing business, ifyaknowwhatimsaying. 
            */
            return expr.Parameters[0].Type.GetProperty(GetExpressionText(expr)).GetCustomAttributes(type, false).Length != 0;
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

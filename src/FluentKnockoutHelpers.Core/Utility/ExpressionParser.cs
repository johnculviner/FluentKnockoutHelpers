using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FluentKnockoutHelpers.Core.Utility
{
    public static class ExpressionParser
    {
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
                return propertyName;

            return propMetadata.DisplayName ?? propMetadata.PropertyName;
        }
    }
}

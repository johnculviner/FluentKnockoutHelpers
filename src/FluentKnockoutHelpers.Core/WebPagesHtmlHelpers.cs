using System.Web.WebPages;
using System.Web.WebPages.Html;
using FluentKnockoutHelpers.Core.Builders;

namespace FluentKnockoutHelpers.Core
{
    /// <summary>
    /// This class contains html helpers for web pages
    /// </summary>
    public static class WebPagesHtmlHelpers
    {
        /// <summary>
        /// Extension method to assign a model to be accessible in the knockout helper
        /// </summary>
        /// <typeparam name="TKoModel">the model</typeparam>
        /// <param name="this">WebPageBase</param>
        /// <returns></returns>
        public static Builder<TKoModel> KnockoutHelperForType<TKoModel>(this WebPageBase @this)
        {
            return new Builder<TKoModel>(@this);
        }

        /// <summary>
        /// Extension method to assign a view model to be accessible in the knockout helper and return just the single property
        /// </summary>
        /// <typeparam name="TKoModel">the model</typeparam>
        /// <param name="this">WebPageBase</param>
        /// <param name="viewModelPropertyName">the property to return</param>
        /// <param name="viewModelPropertyIsObservable">specifies whether the property to return should be observable</param>
        /// <returns></returns>
        public static Builder<TKoModel> KnockoutHelperForType<TKoModel>(this WebPageBase @this, string viewModelPropertyName, bool viewModelPropertyIsObservable)
        {
            return new Builder<TKoModel>(@this, viewModelPropertyName + (viewModelPropertyIsObservable ? "()" : string.Empty));
        }
    }
}
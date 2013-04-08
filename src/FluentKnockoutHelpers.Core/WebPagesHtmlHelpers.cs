using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
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
        /// Create a Builder bound to a .NET type. Use if the root of your KO ViewModel *IS* the .NET type.
        /// </summary>
        /// <typeparam name="TKoModel"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static Builder<TKoModel> KnockoutHelperForType<TKoModel>(this WebPageBase @this)
        {
            return new Builder<TKoModel>(@this);
        }

        /// <summary>
        /// Create a Builder bound to a .NET type. Use if your KO ViewModel has a child property that is your .NET type (viewModelPropertyName)
        /// </summary>
        /// <typeparam name="TKoModel"></typeparam>
        /// <param name="this"></param>
        /// <param name="viewModelPropertyName">The property name of your .NET type off of the root of your KO ViewModel</param>
        /// <param name="viewModelPropertyIsObservable">Is the property observable?</param>
        /// <returns></returns>
        public static Builder<TKoModel> KnockoutHelperForType<TKoModel>(this WebPageBase @this, string viewModelPropertyName, bool viewModelPropertyIsObservable)
        {
            return new Builder<TKoModel>(@this, ResolveViewModelPropertyName(viewModelPropertyName, viewModelPropertyIsObservable));
        }

        /// <summary>
        /// Create a Builder bound to an *Enumerable* .NET type. Use if the root of your KO ViewModel *IS* the .NET type.
        /// </summary>
        /// <typeparam name="TKoModel"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static EnumerableBuilder<TKoModel> KnockoutHelperForEnumerableType<TKoModel>(this WebPageBase @this)
        {
            return new EnumerableBuilder<TKoModel>(@this);
        }


        /// <summary>
        /// Create a Builder bound to an *Enumerable* .NET type. Use if your KO ViewModel has a child property that is your .NET type (viewModelPropertyName)
        /// </summary>
        /// <typeparam name="TKoModel"></typeparam>
        /// <param name="this"></param>
        /// <param name="viewModelPropertyName">The property name of your .NET type off of the root of your KO ViewModel</param>
        /// <param name="viewModelPropertyIsObservable">Is the property observable?</param>
        /// <returns></returns>
        public static EnumerableBuilder<TKoModel> KnockoutHelperForEnumerableType<TKoModel>(this WebPageBase @this, string viewModelPropertyName, bool viewModelPropertyIsObservable)
        {
            return new EnumerableBuilder<TKoModel>(@this, ResolveViewModelPropertyName(viewModelPropertyName, viewModelPropertyIsObservable));
        }

        /// <summary>
        /// Return a KnockoutHelperForApi bound to the type of an API. Used to ultimately create a Builder bound to a particular .NET type as JSON.
        /// <para>Call like: KnockoutHelperForApi&lt;TApi&gt;().Endpoint(...</para>
        /// </summary>
        /// <typeparam name="TApi"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static KnockoutHelperForApi<TApi> KnockoutHelperForApi<TApi>(this WebPageBase @this)
        {
            return new KnockoutHelperForApi<TApi>(@this);
        }

        internal static string ResolveViewModelPropertyName(string viewModelPropertyName, bool viewModelPropertyIsObservable)
        {
            return viewModelPropertyName + (viewModelPropertyIsObservable ? "()" : string.Empty);
        }

        public static IHtmlString TypeNameFor<TType>(this WebPageBase @this)
        {
            var type = typeof(TType);
            return new HtmlString(GlobalSettings.JsonSerializer.SerializerRequiresAssembly
                                ? string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name) : type.FullName);
        }
    }

    public class KnockoutHelperForApi<TApi>
    {
        private WebPageBase _webPageBase;

        public KnockoutHelperForApi(WebPageBase webPageBase)
        {
            _webPageBase = webPageBase;
        }

        /// <summary>
        /// Create a Builder bound to a .NET type that an endpoint returns. Use if the root of your KO ViewModel *IS* the .NET type.
        /// <para>NOTE: For strong typing purposes only. As long as it compiles you can transform the expression however you want</para>
        /// </summary>
        /// <typeparam name="TKoModel"></typeparam>
        /// <param name="endpointExpr"></param>
        /// <returns></returns>
        public Builder<TKoModel> Endpoint<TKoModel>(Expression<Func<TApi, TKoModel>> endpointExpr)
        {
            return _webPageBase.KnockoutHelperForType<TKoModel>();
        }

        /// <summary>
        /// Create a Builder bound to a .NET type that an endpoint returns. Use if your KO ViewModel has a child property that is your .NET type (viewModelPropertyName)
        /// <para>NOTE: For strong typing purposes only. As long as it compiles you can transform the expression however you want</para>
        /// </summary>
        /// <typeparam name="TKoModel"></typeparam>
        /// <param name="endpointExpr"></param>
        /// <param name="viewModelPropertyName">The property name of your .NET type off of the root of your KO ViewModel</param>
        /// <param name="viewModelPropertyIsObservable">Is the property observable?</param>
        /// <returns></returns>
        public Builder<TKoModel> Endpoint<TKoModel>(Expression<Func<TApi, TKoModel>> endpointExpr, string viewModelPropertyName, bool viewModelPropertyIsObservable)
        {
            return _webPageBase.KnockoutHelperForType<TKoModel>(viewModelPropertyName, viewModelPropertyIsObservable);
        }

        /// <summary>
        /// Create a Builder bound to an Enumerable .NET type that an endpoint returns. Use if the root of your KO ViewModel *IS* the .NET type.
        /// <para>NOTE: For strong typing purposes only. As long as it compiles you can transform the expression however you want</para>
        /// </summary>
        /// <typeparam name="TKoModel"></typeparam>
        /// <param name="endpointExpr"></param>
        /// <returns></returns>
        public EnumerableBuilder<TKoModel> Endpoint<TKoModel>(Expression<Func<TApi, IEnumerable<TKoModel>>> endpointExpr)
        {
            return _webPageBase.KnockoutHelperForEnumerableType<TKoModel>();
        }

        /// <summary>
        /// Create a Builder bound to an Enumerable .NET type that an endpoint returns. Use if your KO ViewModel has a child property that is your .NET type (viewModelPropertyName)
        /// <para>NOTE: For strong typing purposes only. As long as it compiles you can transform the expression however you want</para>
        /// </summary>
        /// <typeparam name="TKoModel"></typeparam>
        /// <param name="endpointExpr"></param>
        /// <param name="viewModelPropertyName">The property name of your .NET type off of the root of your KO ViewModel</param>
        /// <param name="viewModelPropertyIsObservable">Is the property observable?</param>
        /// <returns></returns>
        public EnumerableBuilder<TKoModel> Endpoint<TKoModel>(Expression<Func<TApi, IEnumerable<TKoModel>>> endpointExpr, string viewModelPropertyName, bool viewModelPropertyIsObservable)
        {
            return _webPageBase.KnockoutHelperForEnumerableType<TKoModel>(viewModelPropertyName, viewModelPropertyIsObservable);
        }
    }
}
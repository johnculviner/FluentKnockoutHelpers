using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentKnockoutHelpers.Core;
using Newtonsoft.Json;

namespace SurveyApp.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            JsonSerializerConfig.Configure(GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
            FluentKnockoutHelpers.Core.GlobalSettings.JsonSerializer = new JsonDotNetSerializer();
        }
    }

    /// <summary>
    /// Use JSON.NET for serialization for FluentKnockoutHelpers
    /// </summary>
    public class JsonDotNetSerializer : IJsonSerializer
    {
        public string ToJsonString(object toSerialize)
        {
            var serializer = JsonSerializer.Create(GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);

            var tw = new StringWriter();
            serializer.Serialize(tw, toSerialize);
            return tw.ToString();
        }
    }
}
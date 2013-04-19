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
using FluentKnockoutHelpers.Core.TypeMetadata;
using Newtonsoft.Json;
using SurveyApp.Web.App_Start;

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

            FluentKnockoutHelpersConfig.Configure();
        }
    }
}
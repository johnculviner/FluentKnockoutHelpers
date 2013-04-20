using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace SurveyApp.Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("API Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("RPC", "rpc/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
        }
    }
}

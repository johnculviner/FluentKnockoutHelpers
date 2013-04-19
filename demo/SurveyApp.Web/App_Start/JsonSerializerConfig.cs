using Newtonsoft.Json;

namespace SurveyApp.Web.App_Start
{
    public static class JsonSerializerConfig
    {
        public static void Configure(JsonSerializerSettings settings)
        {
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //instruct JSON.NET to serialize type information for all objects
            //allows for client validation and type hierarchies to work
            settings.TypeNameHandling = TypeNameHandling.Objects;
        }
    }
}
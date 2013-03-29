using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SurveyApp.Web
{
    public static class JsonSerializerConfig
    {
        public static void Configure(JsonSerializerSettings settings)
        {
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //instruct JSON.NET to serialize type information
            //when needed for de-serialization. ex: abstract classes
            settings.TypeNameHandling = TypeNameHandling.Auto;
        }
    }
}
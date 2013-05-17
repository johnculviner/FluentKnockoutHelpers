using FluentKnockoutHelpers.Core.Settings;
using FluentKnockoutHelpers.Core.TypeMetadata;

namespace FluentKnockoutHelpers.Core
{
    /// <summary>
    /// The global settings of fluent knockout helpers
    /// </summary>
    public static class GlobalSettings
    {
        static GlobalSettings()
        {
            JsonSerializer = new DefaultJsonSerializer();
            RequiredTokenSettings = new RequiredTokenSettings();
        }

        /// <summary>
        /// Supply a custom IJsonSerializer to use something other than the default JSON.NET. Ex: ServiceStack
        /// </summary>
        public static IJsonSerializer JsonSerializer { get; set; }

        /// <summary>
        /// Required token settings to be used on labels for required ex: "First Name *"
        /// </summary>
        public static RequiredTokenSettings RequiredTokenSettings { get; set; }
    }
}
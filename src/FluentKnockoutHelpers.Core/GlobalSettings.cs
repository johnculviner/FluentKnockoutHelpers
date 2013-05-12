using FluentKnockoutHelpers.Core.TypeMetadata;

namespace FluentKnockoutHelpers.Core
{
    /// <summary>
    /// The global settings of fluent knockout helpers
    /// </summary>
    public static class GlobalSettings
    {
        private static IJsonSerializer _jsonSerializer = new DefaultJsonSerializer();
        private static string _requiredToken = "*";
        private static bool _useRequiredToken = true;

        /// <summary>
        /// Supply a custom IJsonSerializer to use something other than the default JSON.NET. Ex: ServiceStack
        /// </summary>
        public static IJsonSerializer JsonSerializer
        {
            get { return _jsonSerializer; }
            set { _jsonSerializer = value; }
        }

        /// <summary>
        /// Supply a custom required token to use something other than the default "*"
        /// </summary>
        public static string RequiredToken
        {
            get { return _requiredToken; }
            set { _requiredToken = value; }
        }

        /// <summary>
        /// Set whether or not to use a required token on the display name for non-nullable/required properties
        /// </summary>
        public static bool UseRequiredToken
        {
            get { return _useRequiredToken; }
            set { _useRequiredToken = value; }
        }
    }
}
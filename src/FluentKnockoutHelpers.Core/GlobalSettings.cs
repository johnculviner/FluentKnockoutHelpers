namespace FluentKnockoutHelpers.Core
{
    /// <summary>
    /// The global settings of fluent knockout helpers
    /// </summary>
    public static class GlobalSettings
    {
        private static IJsonSerializer _jsonSerializer = new DefaultJsonSerializer();

        /// <summary>
        /// Supply a custom IJsonSerializer to use something other than the built-in MS default. Ex: Json.NET or ServiceStack
        /// </summary>
        public static IJsonSerializer JsonSerializer
        {
            get { return _jsonSerializer; }
            set { _jsonSerializer = value; }
        }
    }
}
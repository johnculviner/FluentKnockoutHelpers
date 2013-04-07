using System.Web.Script.Serialization;
using FluentKnockoutHelpers.Core.TypeMetadata;

namespace FluentKnockoutHelpers.Core
{
    /// <summary>
    /// The default json serializer
    /// </summary>
    public class DefaultJsonSerializer : IJsonSerializer
    {
        private static readonly JavaScriptSerializer _javaScriptSerializer = new JavaScriptSerializer();

        /// <summary>
        /// returns a json string of the serialized object
        /// </summary>
        /// <param name="toSerialize">the object to serialize</param>
        /// <returns></returns>
        public string ToJsonString(object toSerialize)
        {
            return _javaScriptSerializer.Serialize(toSerialize);
        }
    }
}
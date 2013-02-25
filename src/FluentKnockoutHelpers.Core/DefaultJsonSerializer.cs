using System.Web.Script.Serialization;

namespace FluentKnockoutHelpers.Core
{
    public class DefaultJsonSerializer : IJsonSerializer
    {
        private static readonly JavaScriptSerializer _javaScriptSerializer = new JavaScriptSerializer();

        public string ToJsonString(object toSerialize)
        {
            return _javaScriptSerializer.Serialize(toSerialize);
        }
    }
}
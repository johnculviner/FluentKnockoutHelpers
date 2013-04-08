using System;
using System.Web.Script.Serialization;
using FluentKnockoutHelpers.Core.TypeMetadata;

namespace FluentKnockoutHelpers.Core
{
    /// <summary>
    /// The default json serializer
    /// </summary>
    public class DefaultJsonSerializer : IJsonSerializer
    {
        /// <summary>
        /// returns a json string of the serialized object
        /// </summary>
        /// <param name="toSerialize">the object to serialize</param>
        /// <returns></returns>
        public string ToJsonString(object toSerialize)
        {
            var jss = new JavaScriptSerializer();
            return jss.Serialize(toSerialize);
        }

        public bool SerializerRequiresAssembly
        {
            get
            {
                throw new InvalidOperationException("The stock .NET JavaScriptSerializer doesn't support type information. You must use JSON.NET, ServiceStack etc. serializer to support this functionality.");    
            }
        }
    }
}
namespace FluentKnockoutHelpers.Core.TypeMetadata
{
    /// <summary>
    /// interface to json string serializer
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// serialize an object to a json string
        /// </summary>
        /// <param name="toSerialize">the object to serialize</param>
        /// <returns></returns>
        string ToJsonString(object toSerialize);
    }
}
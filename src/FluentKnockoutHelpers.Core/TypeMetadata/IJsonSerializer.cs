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

        /// <summary>
        /// does the serializer require the assembly for type information? EX: json.net = true, servicestack = false
        /// </summary>
        bool SerializerRequiresAssembly { get; }
    }
}
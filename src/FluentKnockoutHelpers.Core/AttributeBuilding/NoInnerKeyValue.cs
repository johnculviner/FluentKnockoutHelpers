namespace FluentKnockoutHelpers.Core.AttributeBuilding
{
    /// <summary>
    /// This class contains an attribute with a single value (no pairs, inner keys and values)
    /// </summary>
    public class NoInnerKeyValue : HtmlAttribute
    {
        /// <summary>
        /// the attribute value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Constructs a new attribute with the key and single value (no inner key/values)
        /// </summary>
        /// <param name="attrKey">the attribute key</param>
        /// <param name="attrValue">the attribute value</param>
        public NoInnerKeyValue(string attrKey, string attrValue)
        {
            Key = attrKey;
            Value = attrValue;
        }

        /// <summary>
        /// Returns the string of the attribute with value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}=\"{1}\"", Key, Value);
        }
    }
}
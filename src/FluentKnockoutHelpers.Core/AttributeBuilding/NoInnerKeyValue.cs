namespace FluentKnockoutHelpers.Core.AttributeBuilding
{
    public class NoInnerKeyValue : HtmlAttribute
    {
        public string Value { get; set; }

        public NoInnerKeyValue(string attrKey, string attrValue)
        {
            Key = attrKey;
            Value = attrValue;
        }

        public override string ToString()
        {
            return string.Format("{0}=\"{1}\"", Key, Value);
        }
    }
}
using System.Collections.Generic;
using System.Text;

namespace FluentKnockoutHelpers.Core.AttributeBuilding
{
    public class InnerKeyValue : HtmlAttribute
    {
        public List<KeyValuePair<string, string>> InnerKvps { get; set; }
        public string PairSeparator { get; set; }
        public string OuterDelimeter { get; set; }

        public InnerKeyValue(string attrKey, string innerKey, string innerValue, string pairSeparator, string outerDelimeter)
        {
            Key = attrKey;
            InnerKvps = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(innerKey, innerValue) };
            PairSeparator = pairSeparator;
            OuterDelimeter = outerDelimeter;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(string.Format("{0}=\"", Key));

            for (int i = 0; i < InnerKvps.Count; i++)
            {
                sb.Append(InnerKvps[i].Key);
                sb.Append(PairSeparator);
                sb.Append(InnerKvps[i].Value);

                if(i != InnerKvps.Count - 1)
                    sb.Append(OuterDelimeter);
            }

            sb.Append("\"");

            return sb.ToString();
        }
    }
}
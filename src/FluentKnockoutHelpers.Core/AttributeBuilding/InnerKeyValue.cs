using System.Collections.Generic;
using System.Text;

namespace FluentKnockoutHelpers.Core.AttributeBuilding
{
    /// <summary>
    /// This class is the attribute inner key value pair
    /// </summary>
    public class InnerKeyValue : HtmlAttribute
    {
        /// <summary>
        /// The list of the attributes key value pairs
        /// </summary>
        public List<KeyValuePair<string, string>> InnerKvps { get; set; }

        /// <summary>
        /// the key value pair separator
        /// </summary>
        public string PairSeparator { get; set; }

        /// <summary>
        /// the outer delimeter of key value pairs
        /// </summary>
        public string OuterDelimeter { get; set; }

        /// <summary>
        /// Construts a new inner key value 
        /// </summary>
        /// <param name="attrKey">the attribute key</param>
        /// <param name="innerKey">the inner key</param>
        /// <param name="innerValue">the inner value</param>
        /// <param name="pairSeparator">the key value pair separator</param>
        /// <param name="outerDelimeter">the outer deliminator of key value pairs</param>
        public InnerKeyValue(string attrKey, string innerKey, string innerValue, string pairSeparator, string outerDelimeter)
        {
            Key = attrKey;
            InnerKvps = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(innerKey, innerValue) };
            PairSeparator = pairSeparator;
            OuterDelimeter = outerDelimeter;
        }

        /// <summary>
        /// returns the string of the attributes value
        /// </summary>
        /// <returns></returns>
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
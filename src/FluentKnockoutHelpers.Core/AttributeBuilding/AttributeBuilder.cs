using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentKnockoutHelpers.Core.AttributeBuilding
{
    public class AttributeBuilder
    {
        //list instead of dictionary use to allow attribute emission in developer specified order (kind of nice when looking at HTML source)
        private readonly List<HtmlAttribute> _attrs = new List<HtmlAttribute>();

        public void Attr(string attrKey, string attrValue)
        {
            Ensure.NotNullEmptyOrWhiteSpace(attrKey, "attrKey");
            Ensure.NotNullEmptyOrWhiteSpace(attrValue, "attrValue");

            ValidateArg(attrKey, "attrKey");
            ValidateArg(attrValue, "attrValue");


            attrKey = attrKey.ToLowerInvariant();

            //TODO make this list configurable 
            switch (attrKey)
            {
                case "id":
                    NoInnerKeyValue_OneSetOnlyAllowed(attrKey, attrValue);
                    break;
                case "class":
                    NoInnerKeyValue_AppendingSpaceSet(attrKey, attrValue);
                    break;
                default:
                    NoInnerKeyValue_AppendingSpaceSet(attrKey, attrValue);
                    break;
            }

        }

        public void Attr(string attrKey, string innerKey, string innerValue)
        {
            Ensure.NotNullEmptyOrWhiteSpace(attrKey, "attrKey");
            Ensure.NotNullEmptyOrWhiteSpace(innerKey, "innerKey");
            Ensure.NotNullEmptyOrWhiteSpace(innerValue, "innerValue");

            ValidateArg(attrKey, "attrKey");
            ValidateArg(innerKey, "innerKey");
            ValidateArg(innerValue, "innerValue");

            attrKey = attrKey.ToLowerInvariant();

            //TODO make this list configurable 
            switch (attrKey)
            {
                case "style":
                    InnerKeyValue_MultiInnerKeyNotAllowed(attrKey, innerKey, innerValue, ": ", "; ");
                    break;
                case "data-bind":
                    InnerKeyValue_MultiInnerKeyNotAllowed(attrKey, innerKey, innerValue, ": ", ", ");
                    break;
            }
        }

        private static void ValidateArg(string argValue, string argName)
        {
            if (argValue.Contains("\""))
                throw new ArgumentOutOfRangeException(argName, argValue, "The passed arguments cannot contain a \" since it wraps attributes");
        }

        #region AttrFuncs

        //ex id="foo"
        private void NoInnerKeyValue_OneSetOnlyAllowed(string attrKey, string attrValue)
        {
            if (_attrs.Exists(x => attrKey.Equals(x.Key)))
                throw new ArgumentOutOfRangeException("attrKey", attrKey,
                                                      "The key already exists as an attribute and is only allowed once");

            _attrs.Add(new NoInnerKeyValue(attrKey, attrValue));
        }

        //ex class="foo bar"
        private void NoInnerKeyValue_AppendingSpaceSet(string attrKey, string attrValue)
        {
            var key = _attrs.SingleOrDefault(x => attrKey.Equals(x.Key));

            if (key == null)
                _attrs.Add(new NoInnerKeyValue(attrKey, attrValue));
            else
                ((NoInnerKeyValue)key).Value += " " + attrValue; ;
        }

        //ex style="background-color: green; ..." OR data-bind="visible: foo(), ..."
        private void InnerKeyValue_MultiInnerKeyNotAllowed(string attrKey, string innerKey, string innerValue, string pairSeparator, string outerDelimeter)
        {
            var key = _attrs.SingleOrDefault(x => attrKey.Equals(x.Key));

            if (key == null)
                _attrs.Add(new InnerKeyValue(attrKey, innerKey, innerValue, pairSeparator, outerDelimeter));
            else
            {
                var innerKeyValue = ((InnerKeyValue)key);

                if (innerKeyValue.InnerKvps.Any(inner => inner.Key.Equals(innerKey)))
                    throw new ArgumentOutOfRangeException("innerKey", innerKey,
                                      "The innerKey already exists for the given attribute and is only allowed once");
                
                innerKeyValue.InnerKvps.Add(new KeyValuePair<string, string>(innerKey, innerValue));
            }
        }

        #endregion

        public virtual string GetContents()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < _attrs.Count; i++)
            {
                sb.Append(_attrs[i]);

                if(i != _attrs.Count - 1)
                    sb.Append(" ");
            }
            return sb.ToString();
        }
    }
}
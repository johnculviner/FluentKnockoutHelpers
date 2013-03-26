using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentKnockoutHelpers.Core.AttributeBuilding
{
    /// <summary>
    /// This class builds an attribute for use in an html tag
    /// </summary>
    public class AttributeBuilder
    {
        //list instead of dictionary use to allow attribute emission in developer specified order (kind of nice when looking at HTML source)
        private readonly List<HtmlAttribute> _attrs = new List<HtmlAttribute>();

        //Signals to the attribute builder that it is the 'special' KoComment mode. In this mode it
        //will only accept data-binds. Furthermore data-binds are written directly to the element
        //and do not go in a data-bind attribute as it is the syntax for knockout comments.
        //Can't think of a better way to do this without making a huge mess of the code for this exception case
        //which would probably end up making all of it harder to maintain and understand
        protected bool InKoCommentMode = false;

        /// <summary>
        /// This will set the attribute's key and value.
        /// Validates that only one 'id' and 'class' attribute exists
        /// </summary>
        /// <param name="attrKey">the attribute key</param>
        /// <param name="attrValue">the attribute value</param>
        public void Attr(string attrKey, string attrValue)
        {
            Ensure.NotNullEmptyOrWhiteSpace(attrKey, "attrKey");
            Ensure.NotNullEmptyOrWhiteSpace(attrValue, "attrValue");

            ValidateForKoCommentMode(attrKey);

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

        /// <summary>
        /// This will set the attribute's key and value.
        /// </summary>
        /// <param name="attrKey">the attribute key</param>
        /// <param name="innerKey">the inner key</param>
        /// <param name="innerValue">the inner value</param>
        public void Attr(string attrKey, string innerKey, string innerValue)
        {
            Ensure.NotNullEmptyOrWhiteSpace(attrKey, "attrKey");
            Ensure.NotNullEmptyOrWhiteSpace(innerKey, "innerKey");
            Ensure.NotNullEmptyOrWhiteSpace(innerValue, "innerValue");

            ValidateForKoCommentMode(attrKey);

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
                default: //"data-bind" and anything else..
                    InnerKeyValue_MultiInnerKeyNotAllowed(attrKey, innerKey, innerValue, ": ", ", ");
                    break;
            }
        }

        //see comment on InKoCommentMode above for rationalization
        private void ValidateForKoCommentMode(string attrKey)
        {
            if(InKoCommentMode && attrKey.ToLowerInvariant() != "data-bind")
                throw new ArgumentException(string.Format(
@"You are attempting to add an attribute '{0}' to a Knockout comment block. Only data-binds are possible on a Knockout comment block.
Please check your syntax. This is what the block looks like so far to help you find it: '{1}'"
                    , attrKey, GetContents()), attrKey);
        }

        /// <summary>
        /// Validates the arguments of an attribute
        /// </summary>
        /// <param name="argValue">the argument value</param>
        /// <param name="argName">the argument name</param>
        private static void ValidateArg(string argValue, string argName)
        {
            if (argValue.Contains("\""))
                throw new ArgumentOutOfRangeException(argName, argValue, "The passed arguments cannot contain a \" since it wraps attributes");
        }

        #region AttrFuncs

        /// <summary>
        /// This will add a single attribute key and value.  If the attribute exists already it will throw an error.
        /// </summary>
        /// <param name="attrKey">the attribute key</param>
        /// <param name="attrValue">the attribute value</param>
        /// <example>ex id="foo"</example>
        private void NoInnerKeyValue_OneSetOnlyAllowed(string attrKey, string attrValue)
        {
            if (_attrs.Exists(x => attrKey.Equals(x.Key)))
                throw new ArgumentOutOfRangeException("attrKey", attrKey,
                                                      "The key already exists as an attribute and is only allowed once");

            _attrs.Add(new NoInnerKeyValue(attrKey, attrValue));
        }

        /// <summary>
        /// This will add an attribute key and value.  If the attribute exists it will add the value to the current value separated with a space.
        /// </summary>
        /// <param name="attrKey">the attribute key</param>
        /// <param name="attrValue">the attribute value</param>
        /// <example>class="foo bar"</example>
        private void NoInnerKeyValue_AppendingSpaceSet(string attrKey, string attrValue)
        {
            var key = _attrs.SingleOrDefault(x => attrKey.Equals(x.Key));

            if (key == null)
                _attrs.Add(new NoInnerKeyValue(attrKey, attrValue));
            else
                ((NoInnerKeyValue)key).Value += " " + attrValue; ;
        }

        /// <summary>
        /// This will add an attribute key and value.  If the attribute exists, it will try to add the value to the current values and will throw an error if inner key already exists.
        /// </summary>
        /// <param name="attrKey">the attribute key</param>
        /// <param name="innerKey">the attribute inner key</param>
        /// <param name="innerValue">the attribute inner value</param>
        /// <param name="pairSeparator">the inner key/value separator to use</param>
        /// <param name="outerDelimeter">the delimeter to use between new key/value pairs</param>
        /// <example>style="background-color: green; ..." OR data-bind="visible: foo(), ..."</example>
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


        /// <summary>
        /// Get the contained attributes as an html string
        /// </summary>
        /// <returns></returns>
        public virtual string GetContents()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < _attrs.Count; i++)
            {
                sb.Append(_attrs[i]);

                if(i != _attrs.Count - 1)
                    sb.Append(" ");
            }

            if (InKoCommentMode)
            {
                //see rationalization for this in a comment on InKoCommentMode above
                //extract the data-bind attribute value since it isn't needed in Knockout Comment blocks
                var regex = new Regex(@"(?<=\bdata-bind="")[^""]*", RegexOptions.IgnoreCase);
                return regex.Match(sb.ToString()).Value;
            }

            return sb.ToString();;
        }
    }
}
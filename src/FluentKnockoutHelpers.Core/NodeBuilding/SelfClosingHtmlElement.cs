namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    /// <summary>
    /// A self closing element
    /// </summary>
    public class SelfClosingHtmlElement : HtmlNode
    {
        public string TagName { get; set; }
        public string InnerHtml { get; set; }
        public bool WriteFullEndTag { get; set; }

        /// <summary>
        /// A self closing element
        /// <para>&#160;</para>
        /// <para>Example:</para>
        /// <para> &lt;input type="submit" /&gt;</para>
        /// <para>&#160;</para>
        /// </summary>
        /// <param name="tagName">The name of the tag for the element</param>
        public SelfClosingHtmlElement(string tagName)
            : this(tagName, false)
        {
        }

        /// <summary>
        /// A self closing element that will will optionally write an end tag if writeFullEndTag == true
        /// </summary>
        /// <para>&#160;</para>
        /// <para>Example:</para>
        /// <para> &lt;select&gt;&lt;/select&gt;</para>
        /// <para>&#160;</para>
        /// <param name="tagName">The name of the tag to write</param>
        /// <param name="writeFullEndTag">Write a full end tag?</param>
        public SelfClosingHtmlElement(string tagName, bool writeFullEndTag)
            : this(tagName, null)
        {
            WriteFullEndTag = writeFullEndTag;
        }

        public SelfClosingHtmlElement(string tagName, string innerHtml)
        {
            TagName = tagName;
            InnerHtml = innerHtml;
        }


        /// <summary>
        /// Returns the beginning element tag
        /// </summary>
        public override string BeginTagBegin
        {
            get { return string.Format("<{0} ", TagName); }
        }

        /// <summary>
        /// returns the self closing tag
        /// </summary>
        public override string BeginTagEnd
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(InnerHtml) || WriteFullEndTag)
                    return string.Format(">{0}</{1}>", InnerHtml, TagName);
                
                return " />"; 
            }
        }
    }
}
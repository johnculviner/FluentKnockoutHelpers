namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    /// <summary>
    /// A self closing element
    /// </summary>
    public class SelfClosingHtmlElement : HtmlNode
    {
        public string TagName { get; set; }
        public string InnerHtml { get; set; }

        /// <summary>
        /// constuctor
        /// </summary>
        /// <param name="tagName">the name of the element</param>
        public SelfClosingHtmlElement(string tagName)
            : this(tagName, null)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tagName">the name of the element</param>
        /// <param name="innerHtml">the inner html</param>
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
                if (!string.IsNullOrWhiteSpace(InnerHtml))
                    return string.Format(">{0}</{1}>", InnerHtml, TagName);
                
                return " />"; 
            }
        }
    }
}
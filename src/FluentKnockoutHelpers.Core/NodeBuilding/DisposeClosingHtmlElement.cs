namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    /// <summary>
    /// The closing element tag
    /// </summary>
    public class DisposeClosingHtmlElement : DisposeClosingHtmlNode
    {
        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="tagName">the tag name of the element</param>
        public DisposeClosingHtmlElement(string tagName)
        {
            TagName = tagName;
        }

        /// <summary>
        /// Gets or sets the tag name
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Gets the beginning tag of the beginning of the element
        /// </summary>
        public override string BeginTagBegin
        {
            get { return string.Format("<{0} ", TagName); }
        }

        /// <summary>
        /// Gets the ending tag of the beginning of the element
        /// </summary>
        public override string BeginTagEnd
        {
            get { return " >"; }
        }

        /// <summary>
        /// Gets the ending tag of the element
        /// </summary>
        public override string EndTag
        {
            get { return string.Format("</{0}>", TagName); }
        }
    }
}
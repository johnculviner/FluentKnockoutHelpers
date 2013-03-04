namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    /// <summary>
    /// The closing tag of a node
    /// </summary>
    public abstract class DisposeClosingHtmlNode : HtmlNode
    {
        /// <summary>
        /// the closing tag of a node
        /// </summary>
        public abstract string EndTag { get; }
    }
}
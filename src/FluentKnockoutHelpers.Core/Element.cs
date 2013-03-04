using FluentKnockoutHelpers.Core.NodeBuilding;

namespace FluentKnockoutHelpers.Core
{
    /// <summary>
    /// the element in html node
    /// </summary>
    public static class Element
    {
        /// <summary>
        /// returns the self closed tag
        /// </summary>
        /// <param name="tagName">the name of the tag</param>
        /// <returns></returns>
        public static SelfClosingHtmlElement SelfClosing(string tagName)
        {
            return new SelfClosingHtmlElement(tagName);
        }

        /// <summary>
        /// returns the tag with the closing tag
        /// </summary>
        /// <param name="tagName">the name of the tag</param>
        /// <returns></returns>
        public static DisposeClosingHtmlElement DisposeClosing(string tagName)
        {
            return new DisposeClosingHtmlElement(tagName);
        }
    }
}

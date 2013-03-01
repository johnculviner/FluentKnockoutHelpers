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
        public static SelfClosingElement SelfClosing(string tagName)
        {
            return new SelfClosingElement(tagName);
        }

        /// <summary>
        /// returns the tag with the closing tag
        /// </summary>
        /// <param name="tagName">the name of the tag</param>
        /// <returns></returns>
        public static DisposeClosingElement DisposeClosing(string tagName)
        {
            return new DisposeClosingElement(tagName);
        }
    }
}

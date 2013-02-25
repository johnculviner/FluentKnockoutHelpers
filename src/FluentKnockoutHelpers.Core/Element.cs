using FluentKnockoutHelpers.Core.NodeBuilding;

namespace FluentKnockoutHelpers.Core
{
    public static class Element
    {
        public static SelfClosingElement SelfClosing(string tagName)
        {
            return new SelfClosingElement(tagName);
        }

        public static DisposeClosingElement DisposeClosing(string tagName)
        {
            return new DisposeClosingElement(tagName);
        }
    }
}

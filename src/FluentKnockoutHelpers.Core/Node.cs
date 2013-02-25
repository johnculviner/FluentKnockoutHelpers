using System;
using FluentKnockoutHelpers.Core.NodeBuilding;

namespace FluentKnockoutHelpers.Core
{
    public abstract class Node
    {
        public abstract string BeginTagBegin { get; }
        public abstract string BeginTagEnd { get; }

        internal static Node Element(string tag, TagClosingMode closingMode)
        {
            switch (closingMode)
            {
                case TagClosingMode.Self:
                    return new SelfClosingElement(tag);
                case TagClosingMode.OnDispose:
                    return new DisposeClosingElement(tag);
                default:
                    throw new ArgumentOutOfRangeException("closingMode");
            }
        }

        public static Node Element(string tag)
        {
            return Element(tag, TagClosingMode.OnDispose);
        }

        public static KoComment KoComment()
        {
            return new KoComment();
        }
    }

    public enum TagClosingMode
    {
        /// <summary>
        /// tag is self closing 
        /// </summary>
        Self,

        /// <summary>
        /// tag closes at the end of a using(){ } block
        /// </summary>
        OnDispose
    }
}

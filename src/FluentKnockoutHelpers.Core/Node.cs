using System;
using FluentKnockoutHelpers.Core.NodeBuilding;

namespace FluentKnockoutHelpers.Core
{
    public abstract class Node
    {
        public abstract string BeginTagBegin { get; }
        public abstract string BeginTagEnd { get; }

        public static Node Element(string tag)
        {
            return new DisposeClosingElement(tag);
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

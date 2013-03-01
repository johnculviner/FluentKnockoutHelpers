using System;
using FluentKnockoutHelpers.Core.NodeBuilding;

namespace FluentKnockoutHelpers.Core
{
    /// <summary>
    /// This class is an html node and must be inherited
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Gets the beginning of the beginning tag
        /// </summary>
        public abstract string BeginTagBegin { get; }

        /// <summary>
        /// Gets the ending of the beginning tag
        /// </summary>
        public abstract string BeginTagEnd { get; }

        /// <summary>
        /// close the element
        /// </summary>
        /// <param name="tag">the tag</param>
        /// <returns></returns>
        public static Node Element(string tag)
        {
            return new DisposeClosingElement(tag);
        }

        /// <summary>
        /// Get new knockout comment
        /// </summary>
        /// <returns></returns>
        public static KoComment KoComment()
        {
            return new KoComment();
        }
    }

    /// <summary>
    /// the closing mode of the tag
    /// </summary>
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

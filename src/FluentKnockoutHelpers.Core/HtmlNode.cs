using System;
using FluentKnockoutHelpers.Core.NodeBuilding;

namespace FluentKnockoutHelpers.Core
{
    /// <summary>
    /// Represents an HtmlNode, ex. an element or comment block
    /// </summary>
    public abstract class HtmlNode
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
        /// Returns a node representing a Knockout comment block that will write it's end tag on dispose of a using block
        /// </summary>
        /// <returns></returns>
        public static DisposeClosingKoComment DisposeClosingKoComment()
        {
            return new DisposeClosingKoComment();
        }

        /// <summary>
        /// Returns a node representing a Knockout comment block that will immediately write its end tag
        /// </summary>
        /// <returns></returns>
        public static SelfClosingKoComment SelfClosingKoComment()
        {
            return new SelfClosingKoComment();
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

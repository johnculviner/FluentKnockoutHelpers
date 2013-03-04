namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    /// <summary>
    /// A node representing a Knockout comment block that will write it's end tag on dispose of a using block
    /// </summary>
    public class DisposeClosingKoComment : DisposeClosingHtmlNode, IKoComment
    {
        /// <summary>
        /// Gets the beginning comment tag
        /// </summary>
        public override string BeginTagBegin
        {
            get { return "<!-- ko "; }
        }

        /// <summary>
        /// Gets the beginning tag end
        /// </summary>
        public override string BeginTagEnd
        {
            get { return " -->"; }
        }

        /// <summary>
        /// Gets the end comment tag
        /// </summary>
        public override string EndTag
        {
            get { return "<!-- /ko -->"; }
        }
    }
}
namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    /// <summary>
    /// adds a knockout comment
    /// </summary>
    public class KoComment : DisposeClosingNode
    {
        //NOTE: for the purposes of this library comment nodes are treated like non self closing elements for knockout bindings
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
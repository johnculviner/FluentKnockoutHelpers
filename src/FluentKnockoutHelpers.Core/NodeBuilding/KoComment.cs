namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    public class KoComment : DisposeClosingNode
    {
        //NOTE: for the purposes of this library comment nodes are treated like non self closing elements for knockout bindings
        public override string BeginTagBegin
        {
            get { return "<!-- ko "; }
        }

        public override string BeginTagEnd
        {
            get { return " -->"; }
        }

        public override string EndTag
        {
            get { return "<!-- /ko -->"; }
        }
    }
}
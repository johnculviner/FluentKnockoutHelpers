namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    public class DisposeClosingElement : DisposeClosingNode, IElement
    {
        public DisposeClosingElement(string tagName)
        {
            TagName = tagName;
        }

        public string TagName { get; set; }

        public override string BeginTagBegin
        {
            get { return string.Format("<{0} ", TagName); }
        }

        public override string BeginTagEnd
        {
            get { return " >"; }
        }

        public override string EndTag
        {
            get { return string.Format("</{0}>", TagName); }
        }
    }
}
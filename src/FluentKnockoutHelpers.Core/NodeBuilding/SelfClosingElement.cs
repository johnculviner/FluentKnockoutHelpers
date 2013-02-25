namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    public class SelfClosingElement : SelfClosingNode, IElement
    {
        public SelfClosingElement(string tagName)
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
            get { return " />"; }
        }
    }
}
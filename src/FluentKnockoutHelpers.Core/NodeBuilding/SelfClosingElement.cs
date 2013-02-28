namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    public class SelfClosingElement : SelfClosingNode, IElement
    {
        public string TagName { get; set; }
        public string InnerHtml { get; set; }

        public SelfClosingElement(string tagName)
            : this(tagName, null)
        {
        }

        public SelfClosingElement(string tagName, string innerHtml)
        {
            TagName = tagName;
            InnerHtml = innerHtml;
        }


        public override string BeginTagBegin
        {
            get { return string.Format("<{0} ", TagName); }
        }

        public override string BeginTagEnd
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(InnerHtml))
                    return string.Format(">{0}</{1}>", InnerHtml, TagName);
                
                return " />"; 
            }
        }
    }
}
using System;
using System.Text;
using FluentKnockoutHelpers.Core.AttributeBuilding;

namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    /// <summary>
    /// this will build a node
    /// </summary>
    public class NodeBuilder : AttributeBuilder
    {
        private HtmlNode _htmlNode;

        public NodeBuilder(HtmlNode htmlNode)
        {
            _htmlNode = htmlNode;

            //kind of yuck, not sure of a better way though...
            if (htmlNode is IKoComment)
                InKoCommentMode = true;
        }

        public override string GetContents()
        {
            var sb = new StringBuilder();

            sb.Append(_htmlNode.BeginTagBegin);
            sb.Append(base.GetContents());
            sb.Append(_htmlNode.BeginTagEnd);

            return sb.ToString();
        }

        public string GetNodeEnd()
        {
            if (!(_htmlNode is DisposeClosingHtmlNode))
                throw new InvalidOperationException();

            return ((DisposeClosingHtmlNode)_htmlNode).EndTag;
        }
    }
}

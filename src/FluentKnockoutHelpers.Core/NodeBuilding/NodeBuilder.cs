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
        private Node _node;

        public NodeBuilder(Node node)
        {
            _node = node;
        }

        public override string GetContents()
        {
            var sb = new StringBuilder();

            sb.Append(_node.BeginTagBegin);
            sb.Append(base.GetContents());
            sb.Append(_node.BeginTagEnd);

            return sb.ToString();
        }

        public string GetNodeEnd()
        {
            if (!(_node is DisposeClosingNode))
                throw new InvalidOperationException();

            return ((DisposeClosingNode)_node).EndTag;
        }
    }
}

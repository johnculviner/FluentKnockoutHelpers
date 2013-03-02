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

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="node">a node</param>
        public NodeBuilder(Node node)
        {
            _node = node;
        }

        public override string GetContents()

        /// <summary>
        /// Gets a string of the beginning node tag
        /// </summary>
        /// <returns></returns>
        public string GetNodeBegin()
        {
            var sb = new StringBuilder();

            sb.Append(_node.BeginTagBegin);
            sb.Append(base.GetContents());
            sb.Append(_node.BeginTagEnd);

            return sb.ToString();
        }

        /// <summary>
        /// Gets a string of the node with a closing tag
        /// </summary>
        /// <returns></returns>
        public string GetNodeEnd()
        {
            if(!(_node is DisposeClosingNode))
                throw new InvalidOperationException();

            return ((DisposeClosingNode) _node).EndTag;
        }
    }
}

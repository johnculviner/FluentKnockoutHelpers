using System;
using FluentKnockoutHelpers.Core.AttributeBuilding;
using FluentKnockoutHelpers.Core.NodeBuilding;

namespace FluentKnockoutHelpers.Core.Builders
{
    public class DisposableBuilder<TModel> : StringReturningBuilder<TModel>, IDisposable
    {
        public DisposableBuilder(Builder<TModel> builder, AttributeBuilder attributeBuilder)
            : base(builder, attributeBuilder)
        {
            ImmediatelyWriteToResponse(GetNodeBuilder().GetContents());
        }

        private NodeBuilder GetNodeBuilder()
        {
            return (NodeBuilder)AttributeBuilder;
        }

        public void Dispose()
        {
            ImmediatelyWriteToResponse(GetNodeBuilder().GetNodeEnd());
        }
    }
}

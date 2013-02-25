using System;
using FluentKnockoutHelpers.Core.NodeBuilding;

namespace FluentKnockoutHelpers.Core.Builders
{
    public class DisposableBuilder<TModel> : StringReturningBuilder<TModel>, IDisposable
    {
        public DisposableBuilder(Builder<TModel> builder)
            : base(builder)
        {
            ImmediatelyWriteToResponse(GetNodeBuilder().GetNodeBegin());
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

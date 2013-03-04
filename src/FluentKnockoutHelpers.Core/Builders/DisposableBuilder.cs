using System;
using FluentKnockoutHelpers.Core.AttributeBuilding;
using FluentKnockoutHelpers.Core.NodeBuilding;

namespace FluentKnockoutHelpers.Core.Builders
{
    public class DisposableBuilder<TModel> : StringReturningBuilder<TModel>, IDisposable
    {
        private readonly NodeBuilder _disposableNodesBuilder;

        public DisposableBuilder(BuilderBase<TModel> builder, NodeBuilder nodeBuilder)
            : base(builder)
        {
            _disposableNodesBuilder = nodeBuilder;
            ImmediatelyWriteToResponse(_disposableNodesBuilder.GetContents());
        }

        public void Dispose()
        {
            ImmediatelyWriteToResponse(_disposableNodesBuilder.GetNodeEnd());
        }

        protected void ImmediatelyWriteToResponse(string s)
        {
            WebPage.WriteLiteral(s + "\r\n");
        }
    }
}

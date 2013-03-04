using FluentKnockoutHelpers.Core.AttributeBuilding;
using FluentKnockoutHelpers.Core.NodeBuilding;

namespace FluentKnockoutHelpers.Core.Builders
{
    public class ForEachBuilder<TModel> : DisposableBuilder<TModel>
    {
        public ForEachBuilder(BuilderBase<TModel> builder, NodeBuilder attributeBuilder) : base(builder, attributeBuilder)
        {
        }
    }
}

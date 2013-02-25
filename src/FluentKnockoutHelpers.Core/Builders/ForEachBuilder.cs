namespace FluentKnockoutHelpers.Core.Builders
{
    public class ForEachBuilder<TModel, TInner> : DisposableBuilder<TModel>
    {
        public ForEachBuilder(Builder<TModel> builder) : base(builder)
        {
        }
    }
}

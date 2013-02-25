namespace FluentKnockoutHelpers.Core
{
    public interface IJsonSerializer
    {
        string ToJsonString(object toSerialize);
    }
}
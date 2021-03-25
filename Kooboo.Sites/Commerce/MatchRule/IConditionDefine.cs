namespace Kooboo.Sites.Commerce.MatchRule
{
    public interface IConditionDefine
    {
        Comparer[] Comparers { get; }
        string Name { get; }
        ConditionValueType ValueType { get; }
    }
}
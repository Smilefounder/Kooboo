using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kooboo.Sites.Commerce.MatchRule
{
    public interface IConditionDefine
    {
        [JsonProperty(nameof(Comparers), ItemConverterType = typeof(StringEnumConverter))]
        Comparer[] Comparers { get; }
        string Name { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        ConditionValueType ValueType { get; }
    }
}
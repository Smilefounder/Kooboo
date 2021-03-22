using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule
{
    public class Rule
    {
        static ConcurrentDictionary<Type, object> _conditionDefines = new ConcurrentDictionary<Type, object>();

        [JsonConverter(typeof(StringEnumConverter))]
        public MatchingType Type { get; set; }
        public Condition[] Conditions { get; set; }

        public static List<ConditionDefine<T>> GetConditionDefines<T>() where T : class
        {
            return _conditionDefines.GetOrAdd(typeof(T), t =>
            {
                return Kooboo.Lib.IOC.Service.GetInstances<ConditionDefine<T>>();
            }) as List<ConditionDefine<T>>;
        }

        public bool Match<T>(T model) where T : class
        {
            var conditionDefines = GetConditionDefines<T>();

            foreach (var condition in Conditions)
            {
                var conditionDefine = conditionDefines.FirstOrDefault(f => f.Name == condition.Left);
                if (conditionDefine == null) continue;
                var isMatchCondition = conditionDefine.Match(model, condition.Comparer, condition.Right);

                if (Type == MatchingType.All && isMatchCondition == false) return false;
                if (Type == MatchingType.Any && isMatchCondition == true) return true;
            }

            switch (Type)
            {
                case MatchingType.All:
                    return true;
                case MatchingType.Any:
                    return false;
                default:
                    return false;
            }
        }
    }
}

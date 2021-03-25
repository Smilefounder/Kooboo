using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule
{
    public abstract  class TargetModelBase<T> where T : TargetModelBase<T>
    {
        public TargetModelBase()
        {
            if (!(this is T)) throw new Exception($"TagetModel {GetType().Name} must extend TargetModelBase<{GetType().Name}>");
        }

        public bool Match(Rule rule)
        {
            var conditionDefines = Helpers.GetConditionDefines<T>();

            foreach (var condition in rule.Conditions)
            {
                var conditionDefine = conditionDefines.FirstOrDefault(f => f.Name == condition.Left);
                if (conditionDefine == null) continue;
                var isMatchCondition = conditionDefine.Match(this as T, condition.Comparer, condition.Right);

                if (rule.Type == MatchingType.All && isMatchCondition == false) return false;
                if (rule.Type == MatchingType.Any && isMatchCondition == true) return true;
            }

            switch (rule.Type)
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

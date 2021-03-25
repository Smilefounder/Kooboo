using Kooboo.Sites.Commerce.MatchRule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce
{
    public static class Helpers
    {
        static readonly ConcurrentDictionary<Type, object> _conditionDefines = new ConcurrentDictionary<Type, object>();

        public static object GetEnumDescription(Type type)
        {
            return Enum.GetNames(type).Select(s => new
            {
                Name = s,
                Display = s
            });
        }

        public static List<ConditionDefineBase<T>> GetConditionDefines<T>() where T:TargetModelBase<T>
        {
            return _conditionDefines.GetOrAdd(typeof(T), t =>
            {
                return Kooboo.Lib.IOC.Service.GetInstances<ConditionDefineBase<T>>();
            }) as List<ConditionDefineBase<T>>;
        }
    }
}

using Kooboo.Sites.Commerce.MatchRule;
using Kooboo.Sites.Commerce.ViewModels;
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

        public static KeyValuePair<string, string>[] GetSpecifications(ItemDefineViewModel[] productTypes, KeyValuePair<Guid, string>[] products, KeyValuePair<Guid, Guid>[] productSkus)
        {
            var specifications = new List<KeyValuePair<string, string>>();

            foreach (var item in productSkus)
            {
                var productType = productTypes.FirstOrDefault(f => f.Id == item.Key);
                if (productType == null) continue;
                if (productType.Type == ItemDefineViewModel.DefineType.Option)
                {
                    var option = productType.Options.FirstOrDefault(f => f.Key == item.Value);
                    specifications.Add(new KeyValuePair<string, string>(productType.Name, option.Value));
                }
                else if (productType.Type == ItemDefineViewModel.DefineType.Text)
                {
                    var specification = products.FirstOrDefault(f => f.Key == item.Value);
                    specifications.Add(new KeyValuePair<string, string>(productType.Name, specification.Value));
                }
            }

            return specifications.ToArray();
        }
    }
}

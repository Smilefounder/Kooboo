using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.MatchRule;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Cart;
using Kooboo.Sites.Commerce.Models.Product;
using System;
using System.Collections;
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

        public static List<ConditionDefineBase<T>> GetConditionDefines<T>() where T : TargetModelBase<T>
        {
            return _conditionDefines.GetOrAdd(typeof(T), t =>
            {
                return Kooboo.Lib.IOC.Service.GetInstances<ConditionDefineBase<T>>();
            }) as List<ConditionDefineBase<T>>;
        }

        public static KeyValuePair<string, string>[] GetSpecifications(ItemDefineModel[] productTypes, ProductModel.Specification[] products, KeyValuePair<Guid, Guid>[] productSkus)
        {
            var specifications = new List<KeyValuePair<string, string>>();

            foreach (var item in productSkus)
            {
                var productType = productTypes.FirstOrDefault(f => f.Id == item.Key);
                var product = products.FirstOrDefault(f => f.Id == item.Key);
                if (productType == null || product == null) continue;
                var options = productType.Type == ItemDefineModel.DefineType.Option ? productType.Options : product.Options;
                var option = options.FirstOrDefault(f => f.Key == item.Value);
                specifications.Add(new KeyValuePair<string, string>(productType.Name, option.Value));
            }

            return specifications.ToArray();
        }

        public static T FromKscriptModel<T>(object obj)
        {
            //TODO Need to optimize performance
            return JsonHelper.Deserialize<T>(JsonHelper.Serialize(obj));
        }

        public static decimal GetCartItemPrice(CartModel cart, decimal discountAmount, int quantity)
        {
            var percentages = cart.DiscountAmount / cart.Items.Where(w => w.Selected).Sum(s => s.DiscountAmount);
            var price = percentages * discountAmount / quantity;
            return price;
        }

        public static object ToKscriptModel(object obj)
        {
            if (obj == null) return obj;
            var type = obj.GetType();

            if (type.IsEnum || obj is Guid)
            {
                return obj.ToString();
            }

            if (type.IsPrimitive || obj is string) return obj;

            if (obj is IEnumerable)
            {
                var array = obj as IEnumerable;
                var list = new List<object>();

                foreach (var item in array)
                {
                    list.Add(ToKscriptModel(item));
                }

                return list.ToArray();
            }


            return type.GetProperties().ToDictionary(k => k.Name, v => ToKscriptModel(v.GetValue(obj)));
        }
    }
}

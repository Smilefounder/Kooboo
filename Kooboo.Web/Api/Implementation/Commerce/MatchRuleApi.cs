using Kooboo.Api;
using Kooboo.Sites.Commerce;
using Kooboo.Sites.Commerce.MatchRule;
using Kooboo.Sites.Commerce.MatchRule.TargetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class MatchRuleApi : IApi
    {
        public string ModelName => "MatchRule";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public object CategoryDefines()
        {
            return GetDefineDisplay(Helpers.GetConditionDefines<Product>());
        }

        public object PromotionDefines()
        {
            return new
            {
                Order = GetDefineDisplay(Helpers.GetConditionDefines<Order>()),
                OrderItem = GetDefineDisplay(Helpers.GetConditionDefines<OrderItem>()),
            };
        }

        public object GetDefineDisplay(IEnumerable<IConditionDefine> defines)
        {
            return defines.Select(define => new
            {
                define.Name,
                Display = define.Name,
                ValueType = define.ValueType.ToString(),
                Comparers = define.Comparers.Select(s => new
                {
                    Name = s.ToString(),
                    Display = s.ToString()
                })
            });
        }
    }
}

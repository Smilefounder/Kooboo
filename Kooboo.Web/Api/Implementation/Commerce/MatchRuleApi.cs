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
    public class MatchRuleApi : CommerceApi
    {
        public override string ModelName => "MatchRule";

        public object CategoryDefines()
        {
            return Helpers.GetConditionDefines<Product>();
        }

        public object PromotionDefines()
        {
            return new
            {
                Order = Helpers.GetConditionDefines<Order>(),
                OrderItem = Helpers.GetConditionDefines<OrderItem>(),
            };
        }
    }
}

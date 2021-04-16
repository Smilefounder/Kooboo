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

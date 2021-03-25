using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.TargetModels
{
    public class OrderItem : TargetModelBase<OrderItem>
    {
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Guid SkuId { get; set; }
    }
}

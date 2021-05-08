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
        public Guid ProductVariantId { get; set; }
        public decimal Price { get; set; }
        public Guid[] Categories { get; set; }
    }
}

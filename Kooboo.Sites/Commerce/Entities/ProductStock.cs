using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class ProductStock
    {
        public Guid SkuId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public StockType Type { get; set; }

        [NotUpdate]
        public DateTime DateTime { get; set; }
        public Guid? OrderItemId { get; set; }
    }

    public enum StockType
    {
        Adjust = 0,
        Sale = 1,
        Returns = 2,
    }
}

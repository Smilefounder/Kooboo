using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Stock : EntityBase
    {
        public Guid SkuId { get; set; }
        public int Quantity { get; set; }
        public StockType StockType { get; set; }
        public DateTime DateTime { get; set; }
    }

    public enum StockType
    {
        Adjust = 0,
        Sale = 1,
        Returns = 2,
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class OrderItem : EntityBase
    {
        public Guid? SkuId { get; set; }
        public string Title { get; set; }
        public string Specifications { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public Guid? Discount { get; set; }
        public string DiscountDetail { get; set; }
        public int Quantity { get; set; }
        public int State { get; set; }
    }
}

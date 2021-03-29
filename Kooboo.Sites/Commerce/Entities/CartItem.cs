using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class CartItem : EntityBase
    {
        public Guid CustomerId { get; set; }
        public bool Selected { get; set; }
        public Guid ProductId { get; set; }
        public Guid SkuId { get; set; }
        public int Quantity { get; set; }
    }
}

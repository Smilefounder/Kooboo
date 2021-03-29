using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels.Cart
{
    public class CartViewModel
    {
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public KeyValuePair<Guid, string> Promotions { get; set; }
        public CartItemViewModel[] Items { get; set; }

        public class CartItemViewModel
        {
            public Guid Id { get; set; }
            public Guid ProductId { get; set; }
            public Guid SkuId { get; set; }
            public bool Selected { get; set; }
            public decimal Price { get; set; }
            public string ProductName { get; set; }
            public KeyValuePair<string, string>[] Specifications { get; set; }
            public int Quantity { get; set; }
            public decimal Amount { get; set; }
            public decimal DiscountAmount { get; set; }
            public KeyValuePair<Guid, string> Promotions { get; set; }
        }
    }
}

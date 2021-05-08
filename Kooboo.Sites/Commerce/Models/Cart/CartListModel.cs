using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Cart
{
    public class CartListModel
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime LastEditTime => Items.Max(m => m.EditTime);
        public Item[] Items { get; set; }

        public class Item
        {
            public bool Selected { get; set; }
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public Guid ProductVariantId { get; set; }
            public KeyValuePair<string, string>[] Specifications { get; set; }
            public int Quantity { get; set; }
            public DateTime EditTime { get; set; }
        }
    }
}

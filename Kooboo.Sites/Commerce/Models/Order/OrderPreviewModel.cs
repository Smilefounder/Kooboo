using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Order
{
    public class OrderPreviewModel
    {
        public decimal Amount { get; set; }
        public Item[] Items { get; set; }

        public class Item
        {
            public string ProductName { get; set; }
            public KeyValuePair<string, string>[] Specifications { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}

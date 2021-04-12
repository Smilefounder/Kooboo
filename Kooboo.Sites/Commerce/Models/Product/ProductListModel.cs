using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Product
{
    public class ProductListModel
    {
        public Guid Id { get; set; }
        public ProductModel.Image[] Images { get; set; }
        public string Title { get; set; }
        public Guid TypeId { get; set; }
        public string TypeName { get; set; }
        public bool Enable { get; set; }
        public Item[] Items { get; set; }

        public class Item
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public KeyValuePair<string,string>[] Specifications { get; set; }
            public int Stock { get; set; }
            public int Sale { get; set; }
            public dynamic Price { get; set; }
            public bool Enable { get; set; }
            public ProductModel.Image[] Image { get; set; }
        }
    }
}

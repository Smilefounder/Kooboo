using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Product
{
    public class ProductKscriptModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ProductModel.Image[] Images { get; set; }
        public string Description { get; set; }
        public KeyValuePair<string, string>[] Attributes { get; set; }
        public Specification[] Specifications { get; set; }
        public Guid TypeId { get; set; }
        public bool Enable { get; set; }

        public class Specification
        {
            public Guid Id { get; set; }
            public KeyValuePair<Guid, string> Options { get; set; }
        }
    }
}

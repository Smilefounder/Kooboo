using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Product
{
    public class SkuModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public KeyValuePair<string,string>[] Specifications { get; set; }
        public int Stock { get; set; }
    }
}

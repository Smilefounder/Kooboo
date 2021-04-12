using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models.Sku;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Product
{
    public class ProductAggregateModel
    {
        public ProductModel Product { get; set; }
        public SkuModel[] Skus { get; set; }
        public KeyValuePair<Guid,int>[] Stocks { get; set; }
    }
}

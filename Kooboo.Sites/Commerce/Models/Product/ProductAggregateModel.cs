using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models.ProductVariant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Product
{
    public class ProductAggregateModel
    {
        public ProductModel Product { get; set; }
        public ProductVariantModel[] ProductVariants { get; set; }
        public KeyValuePair<Guid,int>[] Stocks { get; set; }
        public Guid[] Categories { get; set; }
    }
}

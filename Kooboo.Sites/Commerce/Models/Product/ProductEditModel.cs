using Kooboo.Sites.Commerce.Models.ProductVariant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Product
{
    public class ProductEditModel : ProductModel
    {
        public ProductEditModel(Entities.Product product) : base(product)
        {
        }

        public ProductVariantDetailModel[] ProductVariants { get; set; }
        public Guid[] Categories { get; set; }
    }
}

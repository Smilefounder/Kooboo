using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.ProductCategory
{
    public class ProductCategoryModel
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public bool ProductEnable { get; set; }
        public bool CategoryEnable { get; set; }
    }
}

using Kooboo.Sites.Commerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels
{
    public class SaveProductViewModel : Entities.Product
    {
        public class SkuViewModel : Sku
        {
            public int Stock { get; set; }
        }
        public SkuViewModel[] Skus { get; set; }
    }
}

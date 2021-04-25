using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Cache
{
    public class ProductCategoryCache : CacheBase<ProductCategory[]>
    {
        public ProductCategoryCache(SiteCommerce commerce) : base(commerce)
        {
            var productTypeService = commerce.Service<ProductTypeService>();
            var productService = commerce.Service<ProductService>();
            var productCategoryService = commerce.Service<ProductCategoryService>();
            productTypeService.OnDeleted += _ => Clear();
            productService.OnDeleted += _ => Clear();
            productCategoryService.OnChanged += _ => Clear();
        }

        protected override ProductCategory[] OnGet()
        {
            return Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                return con.GetList<ProductCategory>().ToArray();
            });
        }
    }
}

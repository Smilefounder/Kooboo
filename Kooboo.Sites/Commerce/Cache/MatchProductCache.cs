using Dapper;
using Kooboo.Sites.Commerce.MatchRule.TargetModels;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Cache
{
    public class MatchProductCache : CacheBase<Product[]>
    {
        public MatchProductCache(SiteCommerce commerce) : base(commerce)
        {
            var productTypeService = commerce.Service<ProductTypeService>();
            var productService = commerce.Service<ProductService>();
            var productVariantService = commerce.Service<ProductVariantService>();
            productTypeService.OnDeleted += _ => Clear();
            productService.OnChanged += _ => Clear();
            productService.OnDeleted += _ => Clear();
            productVariantService.OnChanged += _ => Clear();
            productVariantService.OnDeleted += _ => Clear();
        }

        protected override Product[] OnGet()
        {
            return Commerce.CreateDbConnection().ExecuteTask(con =>
                {
                    return con.Query<Product>(@"
SELECT P.Id,
       P.Title,
       P.TypeId,
       PS.Price,
       PS.Tax
FROM ProductVariant PS
         LEFT JOIN Product P ON P.Id = PS.ProductId
").ToArray();
                });
        }
    }
}

using Kooboo.Sites.Ecommerce.Models;
using System;
using System.Collections.Generic;

namespace Kooboo.Sites.Ecommerce.Service
{
    public interface IProductVariantsService : IEcommerceService<ProductVariants>
    {
        bool DeductStock(Guid id, int quantity);
        List<ProductVariants> ListByProduct(Guid ProductId);
        bool ReturnStock(Guid id, int quantity);
    }
}
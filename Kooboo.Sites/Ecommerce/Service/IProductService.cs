using Kooboo.Sites.Ecommerce.Models;
using System;
using System.Collections.Generic;

namespace Kooboo.Sites.Ecommerce.Service
{
    public interface IProductService : IEcommerceService<Product>
    {
        List<Product> All(int skip, int count);
        List<Product> ByCategory(string CategorykeyIdOrPath, int skip, int count);
        List<Category> CategoryList(Guid ProductId);
        List<Product> Top(int count);
    }
}
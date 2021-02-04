using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class ProductCategory : IApi
    {
        public string ModelName => "ProductCategory";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public Sites.Commerce.Entities.ProductCategory[] List(ApiCall apiCall)
        {
            var productCategoryService = new ProductCategoryService(apiCall.Context);
            return productCategoryService.List();
        }

        public void Save(ApiCall apiCall)
        {
            var productCategoryService = new ProductCategoryService(apiCall.Context);
            productCategoryService.List();
        }
    }
}

using Kooboo.Api;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class CategoryApi : IApi
    {
        public string ModelName => "Category";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public Category[] List(ApiCall apiCall)
        {
            var productCategoryService = new CategoryService(apiCall.Context);
            return productCategoryService.List();
        }

        public void Post(Category[] productCategories, ApiCall apiCall)
        {
            var productCategoryService = new CategoryService(apiCall.Context);
            productCategoryService.Save(productCategories);
        }
    }
}

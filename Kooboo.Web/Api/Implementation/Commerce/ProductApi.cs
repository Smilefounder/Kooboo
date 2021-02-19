using Kooboo.Api;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class ProductApi : IApi
    {
        public string ModelName => "Product";

        public bool RequireSite => true;

        public bool RequireUser => true;

        public void Post(SaveProductViewModel viewModel, ApiCall apiCall)
        {
            new ProductService(apiCall.Context).Save(viewModel);
        }

        public SaveProductViewModel Get(Guid id, ApiCall apiCall)
        {
            return new ProductService(apiCall.Context).Query(id);
        }
    }
}

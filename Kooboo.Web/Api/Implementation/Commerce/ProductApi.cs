using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Product;
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

        public void Post(ProductModel viewModel, ApiCall apiCall)
        {
            new ProductService(apiCall.Context).Save(viewModel);
        }

        public ProductModel Get(Guid id, ApiCall apiCall)
        {
            return new ProductService(apiCall.Context).Query(id);
        }

        public PagedListModel<ProductListModel> List(PagingQueryModel viewModel, ApiCall apiCall)
        {
            return new ProductService(apiCall.Context).Query(viewModel);
        }

        public KeyValuePair<Guid, string>[] KeyValue(ApiCall apiCall)
        {
            return new ProductService(apiCall.Context).KeyValue();
        }

        public SkuModel[] SkuList(ApiCall apiCall, Guid id)
        {
            return new ProductService(apiCall.Context).SkuList(id);
        }
    }
}

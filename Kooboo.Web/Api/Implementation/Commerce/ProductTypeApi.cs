using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class ProductTypeApi : IApi
    {
        public string ModelName => "ProductType";

        public bool RequireSite => true;

        public bool RequireUser => true;

        public ProductTypeModel[] List(ApiCall apiCall)
        {
            return new ProductTypeService(apiCall.Context).List();
        }

        public EditProductTypeModel Get(ApiCall apiCall, Guid id)
        {
            var productType = new ProductTypeService(apiCall.Context).Get(id);
            var hasDependent = new ProductTypeService(apiCall.Context).HasDependent(id);
            return new EditProductTypeModel(productType, hasDependent);
        }

        public void Post(ProductTypeModel viewModel, ApiCall apiCall)
        {
            new ProductTypeService(apiCall.Context).Save(viewModel);
        }

        public void Delete(Guid[] ids, ApiCall apiCall)
        {
            new ProductTypeService(apiCall.Context).Delete(ids);
        }

        public KeyValuePair<Guid, string>[] KeyValue(ApiCall apiCall) {
            return new ProductTypeService(apiCall.Context).KeyValue();
        }
    }
}

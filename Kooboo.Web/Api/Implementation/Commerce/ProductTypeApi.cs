using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using Kooboo.Sites.Commerce.Models.Type;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class ProductTypeApi : IApi
    {
        public string ModelName => "ProductType";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public ProductTypeDetailModel[] List(ApiCall apiCall)
        {
            return new ProductTypeService(apiCall.Context).List();
        }

        public ProductTypeDetailModel Get(ApiCall apiCall, Guid id)
        {
            return new ProductTypeService(apiCall.Context).GetDetail(id);
        }

        public void Post(ProductTypeModel viewModel, ApiCall apiCall)
        {
            new ProductTypeService(apiCall.Context).Save(viewModel);
        }

        public void Delete(Guid[] ids, ApiCall apiCall)
        {
            new ProductTypeService(apiCall.Context).Delete(ids);
        }

        public KeyValuePair<Guid, string>[] KeyValue(ApiCall apiCall)
        {
            return new ProductTypeService(apiCall.Context).KeyValue();
        }
    }
}

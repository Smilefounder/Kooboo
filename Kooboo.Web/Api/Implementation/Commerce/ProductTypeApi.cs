using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using Kooboo.Sites.Commerce.Models.Type;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class ProductTypeApi : CommerceApi
    {
        public override string ModelName => "ProductType";

        public ProductTypeDetailModel[] List(ApiCall apiCall)
        {
            return GetService<ProductTypeService>(apiCall).List();
        }

        public ProductTypeDetailModel Get(ApiCall apiCall, Guid id)
        {
            return GetService<ProductTypeService>(apiCall).GetDetail(id);
        }

        public void Post(ProductTypeModel viewModel, ApiCall apiCall)
        {
            GetService<ProductTypeService>(apiCall).Save(viewModel);
        }

        public void Delete(Guid[] ids, ApiCall apiCall)
        {
            GetService<ProductTypeService>(apiCall).Delete(ids);
        }

        public KeyValuePair<Guid, string>[] KeyValue(ApiCall apiCall)
        {
            return GetService<ProductTypeService>(apiCall).KeyValue();
        }
    }
}

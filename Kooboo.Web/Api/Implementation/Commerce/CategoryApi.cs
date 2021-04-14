using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models.Category;
using System;
using System.Collections.Generic;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class CategoryApi : IApi
    {
        public string ModelName => "Category";

        public bool RequireSite => true;

        public bool RequireUser => true;

        public CategoryListModel[] List(ApiCall apiCall)
        {
            return new CategoryService(apiCall.Context).List();
        }

        public EditCategoryModel Get(ApiCall apiCall, Guid id)
        {
            return new CategoryService(apiCall.Context).Get(id);
        }

        public void Post(EditCategoryModel viewModel, ApiCall apiCall)
        {
            new CategoryService(apiCall.Context).Save(viewModel);
        }

        public void Delete(Guid[] ids, ApiCall apiCall)
        {
            new CategoryService(apiCall.Context).Delete(ids);
        }

        public KeyValuePair<Guid, string>[] KeyValue(ApiCall apiCall)
        {
            return new CategoryService(apiCall.Context).KeyValue();
        }
    }
}

using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models.Category;
using System;
using System.Collections.Generic;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class CategoryApi : CommerceApi
    {
        public override string ModelName => "Category";

        public CategoryListModel[] List(ApiCall apiCall)
        {
            return GetService<CategoryService>(apiCall).List();
        }

        public EditCategoryModel Get(ApiCall apiCall, Guid id)
        {
            return GetService<CategoryService>(apiCall).Get(id);
        }

        public void Post(EditCategoryModel viewModel, ApiCall apiCall)
        {
            GetService<CategoryService>(apiCall).Save(viewModel);
        }

        public void Delete(Guid[] ids, ApiCall apiCall)
        {
            GetService<CategoryService>(apiCall).Delete(ids);
        }

        public KeyValuePair<Guid, string>[] KeyValue(ApiCall apiCall)
        {
            return GetService<CategoryService>(apiCall).KeyValue();
        }
    }
}

using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.ViewModels.Category;
using System;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class CategoryApi : IApi
    {
        public string ModelName => "Category";

        public bool RequireSite => true;

        public bool RequireUser => true;

        public CategoryListViewModel[] List(ApiCall apiCall)
        {
            return new CategoryService(apiCall.Context).List();
        }

        public EditCategoryViewModel Get(ApiCall apiCall, Guid id)
        {
            return new CategoryService(apiCall.Context).Get(id);
        }

        public void Post(EditCategoryViewModel viewModel, ApiCall apiCall)
        {
            new CategoryService(apiCall.Context).Save(viewModel);
        }

        public void Delete(Guid[] ids, ApiCall apiCall)
        {
            new CategoryService(apiCall.Context).Delete(ids);
        }
    }
}

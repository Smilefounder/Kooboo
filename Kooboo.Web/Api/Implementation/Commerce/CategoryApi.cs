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
            return new CategoryService(apiCall.Context).List();
        }

        public Category Get(ApiCall apiCall,Guid id)
        {
            return new CategoryService(apiCall.Context).Get(id);
        }

        public void Post(Category category, ApiCall apiCall)
        {
            new CategoryService(apiCall.Context).Save(category);
        }

        public void Delete(Guid[] ids, ApiCall apiCall)
        {
            new CategoryService(apiCall.Context).Delete(ids);
        }
    }
}

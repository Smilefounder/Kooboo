using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models.Category;
using Kooboo.Sites.Commerce.Services;
using System.Linq;

namespace Kooboo.Sites.Commerce.Cache
{
    public class CategoryCache : CacheBase<CategoryModel[]>
    {
        public CategoryCache(SiteCommerce commerce) : base(commerce)
        {
            var categoryService = commerce.Service<CategoryService>();
            categoryService.OnChanged += _ => Clear();
            categoryService.OnDeleted += _ => Clear();
        }

        protected override CategoryModel[] OnGet()
        {
            return Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                return con.GetList<Category>().Select(s => new CategoryModel(s)).ToArray();
            });
        }
    }
}

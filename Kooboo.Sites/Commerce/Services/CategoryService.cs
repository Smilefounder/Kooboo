using Dapper;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Cache;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class CategoryService : ServiceBase
    {
        readonly SiteCache _cache;

        public CategoryService(RenderContext context) : base(context)
        {
            _cache = CommerceCache.GetCache(context);
        }

        public EditCategoryModel Get(Guid id)
        {
            var category = _cache.GetCategories(Context).FirstOrDefault(f => f.Id == id);
            if (category == null) throw new Exception("Not found Category");
            var products = new Guid[0];

            if (category.Type == Category.AddingType.Manual)
            {
                products = new ProductCategoryService(Context).GetByCategoryId(id);
            }

            return new EditCategoryModel(category, products);
        }

        public CategoryListModel[] List()
        {
            var result = new List<CategoryListModel>();

            foreach (var item in _cache.GetCategories(Context))
            {
                var rule = JsonHelper.Deserialize<MatchRule.Rule>(item.Rule);
                int count = 0;
                var _productCategoryService = new ProductCategoryService(Context);

                switch (item.Type)
                {
                    case Category.AddingType.Manual:
                        count = _productCategoryService.GetByCategoryId(item.Id).Count();
                        break;
                    case Category.AddingType.Auto:
                        count = _cache.GetMatchProducts(Context).Where(c => c.Match(rule)).Select(s => s.Id).Distinct().Count();
                        break;
                    default:
                        break;
                }

                result.Add(new CategoryListModel(item, count));
            }

            return result.ToArray();
        }

        public void Save(EditCategoryModel viewModel)
        {
            DbConnection.ExecuteTask(con =>
            {
                var exist = con.Exist<Category>(viewModel.Id);
                if (exist) con.Update(viewModel.ToCategory());
                else con.Insert(viewModel.ToCategory());
                var _productCategoryService = new ProductCategoryService(Context);
                _productCategoryService.SaveByCategoryId(viewModel.Products, viewModel.Id, con);
                _cache.ClearCategories();
            }, true);
        }

        public void Delete(Guid[] ids)
        {
            DbConnection.ExecuteTask(con => con.DeleteList<Category>(ids));
            _cache.ClearCategories();
        }

        public KeyValuePair<Guid, string>[] KeyValue()
        {
            return _cache.GetCategories(Context).Select(s => new KeyValuePair<Guid, string>(s.Id, s.Name)).ToArray();
        }
    }
}

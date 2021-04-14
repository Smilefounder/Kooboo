using Dapper;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
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
        public CategoryService(RenderContext context) : base(context)
        {
        }

        public EditCategoryModel Get(Guid id)
        {
            using (var con = DbConnection)
            {
                var category = con.Get<Category>(id);
                var products = new Guid[0];

                if (category.Type == Category.AddingType.Manual)
                {
                    products = con.Query<Guid>("SELECT ProductId FROM ProductCategory WHERE CategoryId=@Id", category).ToArray();
                }

                return new EditCategoryModel(category, products);
            }
        }

        public CategoryListModel[] List()
        {
            var result = new List<CategoryListModel>();
            using (var con = DbConnection)
            {
                var entities = con.GetList<Category>();
                var autoEntities = entities.Where(w => w.Type == Category.AddingType.Auto);
                var manualEntities = entities.Where(w => w.Type == Category.AddingType.Manual);

                if (autoEntities.Count() > 0)
                {
                    var productService = new ProductService(Context);

                    foreach (var item in autoEntities)
                    {
                        var rule = JsonHelper.Deserialize<MatchRule.Rule>(item.Rule);
                        var count = productService.MatchList.Where(c => c.Match(rule)).Select(s => s.Id).Distinct().Count();
                        result.Add(new CategoryListModel(item, count));
                    }
                }

                if (manualEntities.Count() > 0)
                {
                    var map = con.Query<KeyValuePair<Guid, int>>(@"
select CategoryId as Key,count() as Value from ProductCategory 
where CategoryId in @Ids
group by CategoryId
", new { Ids = manualEntities.Select(s => s.Id) });

                    foreach (var item in manualEntities)
                    {
                        var count = map.FirstOrDefault(f => f.Key == item.Id).Value;
                        result.Add(new CategoryListModel(item, count));
                    }
                }
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
                con.Execute("DELETE FROM ProductCategory WHERE CategoryId=@Id", viewModel);

                if (viewModel.Type == Category.AddingType.Manual)
                {
                    con.InsertList(viewModel.Products.Select(s => new ProductCategory
                    {
                        CategoryId = viewModel.Id,
                        ProductId = s
                    }));

                }
            }, true);
        }

        public void Delete(Guid[] ids)
        {
            DbConnection.ExecuteTask(con => con.DeleteList<Category>(ids));
        }

        public KeyValuePair<Guid, string>[] KeyValue()
        {
            return DbConnection.ExecuteTask(con =>
            {
                return con.Query<KeyValuePair<Guid, string>>("select Id as Key,Name as Value from Category").ToArray();
            });
        }
    }
}

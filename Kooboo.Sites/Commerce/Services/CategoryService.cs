using FluentValidation;
using Kooboo.Sites.Commerce.Cache;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models.Category;
using Kooboo.Sites.Commerce.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Sites.Commerce.Services
{
    public class CategoryService : ServiceBase
    {
        public CategoryService(SiteCommerce commerce) : base(commerce)
        {
        }

        public EditCategoryModel Get(Guid id)
        {
            var category = Commerce.GetCategories().FirstOrDefault(f => f.Id == id);
            if (category == null) throw new Exception("Not found Category");
            var products = new Guid[0];

            if (category.Type == Category.AddingType.Manual)
            {
                products = new ProductCategoryService(Commerce).GetByCategoryId(id);
            }

            return new EditCategoryModel(category, products);
        }

        public CategoryListModel[] List()
        {
            var result = new List<CategoryListModel>();

            foreach (var item in Commerce.GetCategories())
            {
                int count = 0;
                var _productCategoryService = new ProductCategoryService(Commerce);

                switch (item.Type)
                {
                    case Category.AddingType.Manual:
                        count = _productCategoryService.GetByCategoryId(item.Id).Count();
                        break;
                    case Category.AddingType.Auto:
                        count = Commerce.GetMatchProducts().Where(c => c.Match(item.Rule)).Select(s => s.Id).Distinct().Count();
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
            new EditCategoryModelValidator().ValidateAndThrow(viewModel);

            Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                var exist = con.Exist<Category>(viewModel.Id);
                if (exist) con.Update(viewModel.ToCategory());
                else con.Insert(viewModel.ToCategory());
                var _productCategoryService = new ProductCategoryService(Commerce);
                _productCategoryService.SaveByCategoryId(viewModel.Products, viewModel.Id, con);
                Changed(viewModel.Id);
            }, true);
        }

        public void Delete(Guid[] ids)
        {
            Commerce.CreateDbConnection().ExecuteTask(con => con.DeleteList<Category>(ids));
            Deleted(ids);
        }

        public KeyValuePair<Guid, string>[] KeyValue()
        {
            var cache = Commerce.Cache<CategoryCache>().Data;
            return cache.Select(s => new KeyValuePair<Guid, string>(s.Id, s.Name)).ToArray();
        }
    }
}

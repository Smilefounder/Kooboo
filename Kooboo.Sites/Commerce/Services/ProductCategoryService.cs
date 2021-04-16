using Dapper;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Cache;
using Kooboo.Sites.Commerce.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class ProductCategoryService : ServiceBase
    {
        readonly SiteCache _cache;

        public ProductCategoryService(RenderContext context) : base(context)
        {
            _cache = CommerceCache.GetCache(context);
        }

        public Guid[] GetByProductId(Guid id)
        {
            var list = _cache.GetProductCategories(Context);
            return list.Where(w => w.ProductId == id).Select(s => s.CategoryId).ToArray();
        }

        public Guid[] GetByCategoryId(Guid id)
        {
            var list = _cache.GetProductCategories(Context);
            return list.Where(w => w.CategoryId == id).Select(s => s.ProductId).ToArray();
        }

        public void SaveByProductId(Guid[] categories, Guid id, IDbConnection connection = null)
        {
            (connection ?? DbConnection).ExecuteTask(con =>
            {
                con.Execute("delete from ProductCategory where ProductId=@Id", new { Id = id });

                con.InsertList(categories.Select(s => new ProductCategory
                {
                    CategoryId = s,
                    ProductId = id
                }));

                _cache.ClearProductCategories();

            }, connection == null, connection == null);
        }

        public void SaveByCategoryId(Guid[] productIds, Guid id, IDbConnection connection = null)
        {
            (connection ?? DbConnection).ExecuteTask(con =>
            {
                con.Execute("delete from ProductCategory where CategoryId=@Id", new { Id = id });

                con.InsertList(productIds.Select(s => new ProductCategory
                {
                    CategoryId = id,
                    ProductId = s
                }));

                _cache.ClearProductCategories();

            }, connection == null, connection == null);
        }
    }
}

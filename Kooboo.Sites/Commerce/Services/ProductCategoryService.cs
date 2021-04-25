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
        public ProductCategoryService(SiteCommerce commerce) : base(commerce)
        {
        }

        public Guid[] GetByProductId(Guid id)
        {
            var list = Commerce.Cache<ProductCategoryCache>().Data;
            return list.Where(w => w.ProductId == id).Select(s => s.CategoryId).ToArray();
        }

        public Guid[] GetByCategoryId(Guid id)
        {
            var list = Commerce.Cache<ProductCategoryCache>().Data;
            return list.Where(w => w.CategoryId == id).Select(s => s.ProductId).ToArray();
        }

        public void SaveByProductId(Guid[] categories, Guid id, IDbConnection connection = null)
        {
            (connection ?? Commerce.CreateDbConnection()).ExecuteTask(con =>
            {
                con.Execute("delete from ProductCategory where ProductId=@Id", new { Id = id });

                con.InsertList(categories.Select(s => new ProductCategory
                {
                    CategoryId = s,
                    ProductId = id
                }));

                Changed(id);
            }, connection == null, connection == null);
        }

        public void SaveByCategoryId(Guid[] productIds, Guid id, IDbConnection connection = null)
        {
            (connection ?? Commerce.CreateDbConnection()).ExecuteTask(con =>
            {
                con.Execute("delete from ProductCategory where CategoryId=@Id", new { Id = id });

                con.InsertList(productIds.Select(s => new ProductCategory
                {
                    CategoryId = id,
                    ProductId = s
                }));

                Changed(id);
            }, connection == null, connection == null);
        }
    }
}

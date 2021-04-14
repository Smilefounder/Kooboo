using Dapper;
using Kooboo.Data.Context;
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
        public ProductCategoryService(RenderContext context) : base(context)
        {
        }

        public Guid[] GetByProductId(Guid id)
        {
            using (var con = DbConnection)
            {
                return con.Query<Guid>("select CategoryId from ProductCategory where ProductId=@Id", new { Id = id })
                   .ToArray();
            }
        }

        public void SaveByProductId(Guid[] categories, Guid id, IDbConnection connection = null)
        {
            var con = connection ?? DbConnection;

            try
            {
                IDbTransaction tran = null;
                if (connection == null)
                {
                    con.Open();
                    tran = con.BeginTransaction();
                }

                con.Execute("delete from ProductCategory where ProductId=@Id", new { Id = id });

                con.InsertList(categories.Select(s => new ProductCategory
                {
                    CategoryId = s,
                    ProductId = id
                }));

                if (tran != null) tran.Commit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection == null) con.Dispose();
            }
        }
    }
}

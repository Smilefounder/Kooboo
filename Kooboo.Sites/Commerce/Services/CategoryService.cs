using Dapper;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Entities;
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

        public Category[] List()
        {
            using (var con = DbConnection)
            {
                return con.Get<Category>().ToArray();
            }
        }

        public void Save(Category[] productCategories)
        {
            var newIds = productCategories.Select(s => s.Id);

            using (var con = DbConnection)
            {
                con.Open();
                var tran = con.BeginTransaction();
                var list = List();
                var oldIds = list.Select(s => s.Id);
                con.Delete(list.Where(w => !newIds.Contains(w.Id)));
                con.Insert(productCategories.Where(w => !oldIds.Contains(w.Id)));
                con.Update(productCategories.Where(w => oldIds.Contains(w.Id)));
                tran.Commit();
            }
        }
    }
}

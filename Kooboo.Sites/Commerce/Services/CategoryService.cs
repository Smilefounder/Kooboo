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

        public Category Get(Guid id)
        {
            using (var con = DbConnection)
            {
                return con.Get<Category>(id);
            }
        }

        public Category[] List()
        {
            using (var con = DbConnection)
            {
                return con.Get<Category>().ToArray();
            }
        }

        public void Save(Category category)
        {
            using (var con = DbConnection)
            {
                var entity = con.Get<Category>(category.Id);
                if (entity != null) con.Update(category);
                else con.Insert(category);
            }
        }

        public void Delete(Guid[] ids)
        {
            using (var con = DbConnection)
            {
                con.Delete<Category>(ids);
            }
        }
    }
}

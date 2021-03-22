using Dapper;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class ProductTypeService : ServiceBase
    {
        public ProductTypeService(RenderContext context) : base(context)
        {
        }

        public void Save(ProductTypeViewModel viewModel)
        {
            using (var con = DbConnection)
            {
                con.Open();
                var tran = con.BeginTransaction();
                var exist = con.Exist<ProductType>(viewModel.Id);
                if (exist) con.Update(viewModel.ToEntity());
                else con.Insert(viewModel.ToEntity());
                tran.Commit();
            }
        }

        public ProductTypeViewModel Get(Guid id)
        {
            using (var con = DbConnection)
            {
                var entity = con.Get<ProductType>(id);
                return new ProductTypeViewModel(entity);
            }
        }

        public ProductTypeViewModel[] List()
        {
            using (var con = DbConnection)
            {
                var entities = con.GetList<ProductType>();
                return entities.Select(s => new ProductTypeViewModel(s)).ToArray();
            }
        }

        internal KeyValuePair<Guid, string>[] KeyValue()
        {
            using (var con = DbConnection)
            {
                return con.Query<KeyValuePair<Guid, string>>("select Id as Key,Name as value from ProductType").ToArray();
            }
        }

        public void Delete(Guid[] ids)
        {
            using (var con = DbConnection)
            {
                con.DeleteList<ProductType>(ids);
            }
        }

        public bool HasDependent(Guid id)
        {
            using (var con = DbConnection)
            {
                return con.QuerySingleOrDefault<bool?>("select true from Product where TypeId=@Id limit 1", new { Id = id }) ?? false;
            }
        }
    }
}

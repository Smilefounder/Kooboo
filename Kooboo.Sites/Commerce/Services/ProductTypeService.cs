using Dapper;
using FluentValidation;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Type;
using Kooboo.Sites.Commerce.Validators;
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

        public void Save(ProductTypeModel viewModel)
        {
            new ProductTypeModelValidator().ValidateAndThrow(viewModel);

            using (var con = DbConnection)
            {
                var exist = con.Exist<ProductType>(viewModel.Id);

                if (exist)
                {
                    CheckUpdateRestrain(viewModel);
                    con.Update(viewModel.ToEntity());
                }
                else
                {
                    con.Insert(viewModel.ToEntity());
                }
            }
        }

        private void CheckUpdateRestrain(ProductTypeModel viewModel)
        {
            var old = Get(viewModel.Id);
            if (old.ProductCount == 0) return;

            if (ItemDefineToEqualString(old.Attributes) != ItemDefineToEqualString(viewModel.Attributes))
            {
                throw new Exception("Attributes can't match");
            }

            if (ItemDefineToEqualString(old.Specifications) != ItemDefineToEqualString(viewModel.Specifications))
            {
                throw new Exception("Specifications can't match");
            }
        }

        static string ItemDefineToEqualString(ItemDefineModel[] items)
        {
            return string.Join(string.Empty, items.OrderBy(o => o.Id).Select(s => s.ToString()));
        }

        public ProductTypeDetailModel Get(Guid id)
        {
            using (var con = DbConnection)
            {
                var entity = con.QueryFirstOrDefault(@"
SELECT PT.Id,
       PT.Name,
       PT.attributes,
       PT.specifications,
       SUM(CASE WHEN P.Id ISNULL THEN 0 ELSE 1 END) AS ProductCount
FROM ProductType PT
         LEFT JOIN Product P ON PT.Id = P.TypeId
WHERE PT.Id=@Id
GROUP BY PT.Id
LIMIT 1
", new { Id = id });

                if (entity == null) throw new Exception("Not found product type");

                return new ProductTypeDetailModel
                {
                    Attributes = JsonHelper.Deserialize<ItemDefineModel[]>(entity.Attributes),
                    Id = entity.Id,
                    Name = entity.Name,
                    Specifications = JsonHelper.Deserialize<ItemDefineModel[]>(entity.Specifications),
                    ProductCount = (int)entity.ProductCount
                };
            }
        }

        public ProductTypeDetailModel[] List()
        {
            using (var con = DbConnection)
            {
                var result = new List<ProductTypeDetailModel>();

                var list = con.Query(@"
SELECT PT.Id,
       PT.Name,
       PT.attributes,
       PT.specifications,
       SUM(CASE WHEN P.Id ISNULL THEN 0 ELSE 1 END) AS ProductCount
FROM ProductType PT
         LEFT JOIN Product P ON PT.Id = P.TypeId
GROUP BY PT.ID
");
                foreach (var item in list)
                {
                    result.Add(new ProductTypeDetailModel
                    {
                        Attributes = JsonHelper.Deserialize<ItemDefineModel[]>(item.Attributes),
                        Id = item.Id,
                        Name = item.Name,
                        Specifications = JsonHelper.Deserialize<ItemDefineModel[]>(item.Specifications),
                        ProductCount = (int)item.ProductCount
                    });
                }

                return result.ToArray();
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
    }
}

using Dapper;
using FluentValidation;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models.Product;
using Kooboo.Sites.Commerce.Models.Sku;
using Kooboo.Sites.Commerce.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class ProductSkuService : ServiceBase
    {
        public ProductSkuService(RenderContext context) : base(context)
        {
        }

        public void Save(Guid id, SkuModel[] skus, IDbConnection connection = null)
        {
            new SkuModelsValidator().ValidateAndThrow(skus);

            var con = connection ?? DbConnection;
            IDbTransaction tran = null;

            if (connection == null)
            {
                con.Open();
                tran = con.BeginTransaction();
            }

            var existSkus = con.Query<ProductSku>("select * from ProductSku where ProductId=@Id", new { Id = id }).Select(s => new SkuModel(s));
            var existEqualList = existSkus.Select(s => new KeyValuePair<Guid, string>(s.Id, s.ToEqualString()));
            var equalList = skus.Select(s => new KeyValuePair<Guid, string>(s.Id, s.ToEqualString()));

            var deleteList = new List<Guid>();
            var addList = new List<Models.Sku.SkuModel>();
            var updateList = new List<Models.Sku.SkuModel>();

            foreach (var item in existEqualList)
            {
                if (equalList.All(a => a.Value != item.Value))
                {
                    deleteList.Add(item.Key);
                }
            }

            foreach (var item in skus)
            {
                var equalString = item.ToEqualString();

                var finderItem = existEqualList.FirstOrDefault(f => f.Value == equalString);

                if (finderItem.Key == Guid.Empty)
                {
                    addList.Add(item);
                }
                else
                {
                    item.Id = finderItem.Key;
                    updateList.Add(item);
                }
            }


            con.DeleteList<ProductSku>(deleteList);
            con.InsertList(addList.Select(s => s.ToSku()));
            con.UpdateList(updateList.Select(s => s.ToSku()));

            if (connection == null)
            {
                tran.Commit();
                con.Dispose();
            }
        }

        public SkuDetailModel[] List(Guid productId, IDbConnection connection = null)
        {
            var con = connection;

            var list = con.Query(@"
SELECT PS.Id,
       PS.Specifications,
       PS.ProductId,
       PS.Name,
       PS.Price,
       PS.Tax,
       PS.Image,
       PS.Enable,
       SUM(CASE WHEN S.Quantity IS NULL THEN 0 ELSE S.Quantity END) AS Stock
FROM ProductSku PS
         LEFT JOIN ProductStock S ON PS.Id = S.SkuId
WHERE PS.ProductId=@Id
GROUP BY PS.Id
", new { Id = productId });

            if (connection == null) con.Dispose();

            return list.Select(s => new SkuDetailModel
            {
                Id = s.Id,
                Enable = Convert.ToBoolean(s.Enable),
                Name = s.Name,
                Price = (decimal)s.Price,
                ProductId = s.ProductId,
                Stock = (int)s.Stock,
                Tax = (decimal)s.Tax,
                Image = s.Image == null ? null : JsonHelper.Deserialize<ProductModel.Image>(s.Image),
                Specifications = JsonHelper.Deserialize<KeyValuePair<Guid, Guid>[]>(s.Specifications),
            }).ToArray();

        }
    }
}

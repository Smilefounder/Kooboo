using Dapper;
using FluentValidation;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models.Product;
using Kooboo.Sites.Commerce.Models.ProductVariant;
using Kooboo.Sites.Commerce.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kooboo.Sites.Commerce.Services
{
    public class ProductVariantService : ServiceBase
    {
        public ProductVariantService(SiteCommerce commerce) : base(commerce)
        {
        }

        public void Save(Guid id, ProductVariantModel[] productVariants, IDbConnection connection = null)
        {
            new ProductVariantModelsValidator().ValidateAndThrow(productVariants);

            (connection ?? Commerce.CreateDbConnection()).ExecuteTask(con =>
            {
                var existProductVariants = con.Query<ProductVariant>("select * from ProductVariant where ProductId=@Id", new { Id = id }).Select(s => new ProductVariantModel(s));
                var existEqualList = existProductVariants.Select(s => new KeyValuePair<Guid, string>(s.Id, s.ToEqualString()));
                var equalList = productVariants.Select(s => new KeyValuePair<Guid, string>(s.Id, s.ToEqualString()));

                var deleteList = new List<Guid>();
                var addList = new List<Models.ProductVariant.ProductVariantModel>();
                var updateList = new List<Models.ProductVariant.ProductVariantModel>();

                foreach (var item in existEqualList)
                {
                    if (equalList.All(a => a.Value != item.Value))
                    {
                        deleteList.Add(item.Key);
                    }
                }

                foreach (var item in productVariants)
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

                con.DeleteList<ProductVariant>(deleteList);
                con.InsertList(addList.Select(s => s.ToProductVariant()));
                con.UpdateList(updateList.Select(s => s.ToProductVariant()));
                if (deleteList.Count > 0) Deleted(deleteList.ToArray());
                var changeIds = addList.Union(updateList).Select(s => s.Id).ToArray();
                if (changeIds.Length > 0) Changed(changeIds);
            }, connection == null, connection == null);
        }

        public ProductVariantDetailModel[] List(Guid productId, IDbConnection connection = null)
        {
            return (connection ?? Commerce.CreateDbConnection()).ExecuteTask(con =>
            {
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
FROM ProductVariant PS
         LEFT JOIN ProductStock S ON PS.Id = S.ProductVariantId
WHERE PS.ProductId=@Id
GROUP BY PS.Id
", new { Id = productId });

                return list.Select(s => new ProductVariantDetailModel
                {
                    Id = s.Id,
                    Enable = s.Enable,
                    Name = s.Name,
                    Price = (decimal)s.Price,
                    ProductId = s.ProductId,
                    Stock = (int)s.Stock,
                    Tax = (decimal)s.Tax,
                    Image = s.Image == null ? null : JsonHelper.Deserialize<ProductModel.Image>(s.Image),
                    Specifications = JsonHelper.Deserialize<KeyValuePair<Guid, Guid>[]>(s.Specifications),
                }).ToArray();
            }, connection == null, connection == null);
        }
    }
}

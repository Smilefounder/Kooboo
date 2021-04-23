using Dapper;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Cache;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Cart;
using Kooboo.Sites.Commerce.Models.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class CartService : ServiceBase
    {
        public CartService(SiteCommerce commerce) : base(commerce)
        {
        }

        public CartModel GetCart(Guid customerId, IDbConnection connection = null, bool filterNotSelected = false)
        {
            var con = connection ?? Commerce.CreateDbConnection();
            var whereSelected = filterNotSelected ? "AND CI.Selected=1" : string.Empty;

            var list = con.Query($@"
SELECT CI.ProductId,
       CI.SkuId,
       CI.Id,
       CI.Quantity,
       CI.Selected,
       PS.Price,
       P.Title           AS ProductName,
       P.Specifications  AS ProductSpecifications,
       PS.Specifications AS ProductSkuSpecifications,
       PT.Specifications AS ProductTypeSpecifications,
       SUM(S.Quantity)   AS Stock,
       PT.Id             AS TypeId,
       PS.Tax            AS Tax,
       P.Title           AS Title
FROM CartItem CI
         LEFT JOIN ProductSku PS ON CI.SkuId = PS.Id
         LEFT JOIN Product P ON P.Id = PS.ProductId
         LEFT JOIN ProductType PT ON PT.Id = P.TypeId
         LEFT JOIN ProductStock S ON PS.Id = S.SkuId
WHERE CustomerId = @CustomerId {whereSelected}
GROUP BY CI.SkuId
", new { CustomerId = customerId });

            var cart = new CartModel();
            var items = new List<CartModel.CartItemModel>();

            foreach (var item in list)
            {
                var typeSpecifications = JsonHelper.Deserialize<ItemDefineModel[]>(item.ProductTypeSpecifications);
                var skuSpecifications = JsonHelper.Deserialize<KeyValuePair<Guid, Guid>[]>(item.ProductSkuSpecifications);
                var productSpecifications = JsonHelper.Deserialize<ProductModel.Specification[]>(item.ProductSpecifications);

                items.Add(new CartModel.CartItemModel()
                {
                    Id = item.Id,
                    Price = (decimal)item.Price,
                    Quantity = (int)item.Quantity,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    SkuId = item.SkuId,
                    Selected = item.Selected,
                    Specifications = Helpers.GetSpecifications(typeSpecifications, productSpecifications, skuSpecifications),
                    Stock = Convert.ToInt32(item.Stock),
                    TypeId = item.TypeId,
                    Tax = (decimal)item.Tax,
                    Title = item.Title
                });
            }


            cart.Items = items.ToArray();
            cart.Discount(Commerce);
            if (connection == null) con.Dispose();
            return cart;
        }

        public void DeleteItems(Guid[] ids)
        {
            using (var con = Commerce.CreateDbConnection())
            {
                con.DeleteList<CartItem>(ids);
            }
        }

        public void Post(Guid customerId, Guid skuId, int quantity, bool selected)
        {

            Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                var result = con.QueryFirstOrDefault(@"
SELECT P.ProductId,
       SUM(CASE WHEN PS.Quantity IS NULL THEN 0 ELSE PS.Quantity END) AS Stock,
       CI.Id                                                          AS CartItemId,
       CI.Selected
FROM ProductSku P
         LEFT JOIN ProductStock PS ON P.Id = PS.SkuId
         LEFT JOIN CartItem CI ON P.Id = CI.SkuId AND CI.CustomerId = @CustomerId
WHERE P.Id = @SkuId
GROUP BY P.Id
", new { SkuId = skuId, CustomerId = customerId });


                if (result == null) throw new Exception("Not found sku");
                if (quantity > result.Stock) throw new Exception("Out of stock");

                if (result.CartItemId == null)
                {
                    con.Insert(new CartItem
                    {
                        CustomerId = customerId,
                        EditTime = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                        ProductId = result.ProductId,
                        Quantity = quantity,
                        Selected = selected,
                        SkuId = skuId
                    });

                }
                else
                {
                    con.Update(new CartItem
                    {
                        CustomerId = customerId,
                        EditTime = DateTime.UtcNow,
                        Id = result.CartItemId,
                        ProductId = result.ProductId,
                        Quantity = quantity,
                        Selected = selected,
                        SkuId = skuId
                    });
                }

            }, true);
        }

        public PagedListModel<CartListModel> Query(PagingQueryModel model)
        {
            using (var con = Commerce.CreateDbConnection())
            {
                var result = new PagedListModel<CartListModel>();
                var count = con.QuerySingleOrDefault<int>("SELECT COUNT(1) FROM (SELECT 1 FROM CartItem GROUP BY CustomerId)");
                result.SetPageInfo(model, count);

                var list = con.Query(@"
SELECT CI.Id,
       CI.CustomerId,
       C.UserName,
       CI.Selected,
       CI.ProductId,
       P.Title,
       CI.SkuId,
       CI.Quantity,
       CI.EditTime,
       P.Specifications as ProductSpecifications,
       PS.Specifications as ProductSkuSpecifications,
       PT.Specifications as ProductTypeSpecifications
FROM CartItem CI
         LEFT JOIN ProductSku PS ON PS.Id = CI.SkuId
         LEFT JOIN Product P ON P.Id = CI.ProductId
         LEFT JOIN ProductType PT ON PT.Id = P.TypeId
         LEFT JOIN Customer C ON C.Id = CI.CustomerId
WHERE CustomerId
          IN (SELECT CustomerId
              FROM CartItem
              GROUP BY CustomerId
              ORDER BY EditTime DESC
              LIMIT @PageSize OFFSET @Offset)

", new
                {
                    PageSize = model.Size,
                    Offset = result.GetOffset()
                });

                result.List = list.GroupBy(g => g.CustomerId).Select(s => new CartListModel
                {
                    CustomerId = s.Key,
                    CustomerName = s.Max(m => m.UserName),
                    Items = s.Select(ss =>
                    {
                        var typeSpecifications = JsonHelper.Deserialize<ItemDefineModel[]>(ss.ProductTypeSpecifications);
                        var skuSpecifications = JsonHelper.Deserialize<KeyValuePair<Guid, Guid>[]>(ss.ProductSkuSpecifications);
                        var productSpecifications = JsonHelper.Deserialize<KeyValuePair<Guid, string>[]>(ss.ProductSpecifications);

                        return new CartListModel.Item
                        {
                            EditTime = ss.EditTime,
                            ProductId = ss.ProductId,
                            ProductName = ss.Title,
                            Quantity = (int)ss.Quantity,
                            Selected = Convert.ToBoolean(ss.Selected),
                            SkuId = ss.SkuId,
                            Specifications = Helpers.GetSpecifications(typeSpecifications, productSpecifications, skuSpecifications)
                        };
                    }).ToArray()
                }).OrderByDescending(o => o.Items.Max(m => m.EditTime)).ToList();

                return result;
            }
        }
    }
}

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
        public CartService(RenderContext context) : base(context)
        {
        }

        public void SaveItem(CartItem cartItem)
        {
            using (var con = DbConnection)
            {
                var existEntity = con.QueryFirstOrDefault<CartItem>("select * from CartItem where CustomerId=@CustomerId and SkuId=@SkuId", cartItem);

                if (existEntity != null)
                {
                    existEntity.Selected = cartItem.Selected;
                    existEntity.Quantity = cartItem.Quantity;
                    existEntity.EditTime = DateTime.UtcNow;
                    con.Update(existEntity);
                }
                else
                {
                    cartItem.Id = Guid.NewGuid();
                    cartItem.EditTime = DateTime.UtcNow;
                    con.Insert(cartItem);
                }
            }
        }

        public CartModel GetCart(Guid customerId, IDbConnection connection = null, bool filterNotSelected = false)
        {
            var con = connection ?? DbConnection;
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
       SUM(S.Quantity)   AS Stock
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
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    SkuId = item.SkuId,
                    Selected = Convert.ToBoolean(item.Selected),
                    Specifications = Helpers.GetSpecifications(typeSpecifications, productSpecifications, skuSpecifications),
                    Stock = Convert.ToInt32(item.Stock)
                });
            }

           
            cart.Items = items.ToArray();
            cart.Discount(Context);
            if (connection == null) con.Dispose();
            return cart;
        }

        public void DeleteItems(Guid[] ids)
        {
            using (var con = DbConnection)
            {
                con.DeleteList<CartItem>(ids);
            }
        }

        public PagedListModel<CartListModel> Query(PagingQueryModel model)
        {
            using (var con = DbConnection)
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

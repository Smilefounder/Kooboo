using Dapper;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class OrderService : ServiceBase
    {
        public OrderService(SiteCommerce commerce) : base(commerce)
        {
        }

        public void Create(CreateOrderModel model)
        {
            Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                var cart = new CartService(Commerce).GetCart(model.CustomerId, con, true);
                var consignee = new ConsigneeService(Commerce).Get(model.ConsigneeId, con);
                CheckStock(cart);
                var order = model.ToOrder(cart);
                order.Consignee = JsonHelper.Serialize(consignee);
                var orderItems = new List<Entities.OrderItem>();
                var stocks = new List<Entities.ProductStock>();

                foreach (var item in cart.Items)
                {
                    var orderItem = item.ToOrderItem(order.Id, cart);
                    orderItems.Add(orderItem);

                    stocks.Add(new Entities.ProductStock
                    {
                        DateTime = DateTime.UtcNow,
                        OrderItemId = orderItem.Id,
                        ProductId = orderItem.ProductId,
                        Quantity = -orderItem.Quantity,
                        ProductVariantId = orderItem.ProductVariantId,
                        Type = Entities.StockType.Sale
                    });
                }

                con.Insert(order);
                con.InsertList(orderItems);
                con.InsertList(stocks);
                con.DeleteList<Entities.CartItem>(cart.Items.Select(s => s.Id));
            }, true);
        }

        public OrderDetailModel Get(Guid id)
        {
            return Commerce.CreateDbConnection().ExecuteTask(con =>
             {
                 var order = con.QueryFirstOrDefault(@"
SELECT C.UserName,
       O.CreateTime,
       O.PaymentTime,
       O.PaymentMethod,
       O.Amount,
       O.State,
       O.Promotions,
       O.Consignee
FROM 'Order' O
         LEFT JOIN Customer C ON C.Id = O.CustomerId
WHERE O.Id = @Id
LIMIT 1
", new { Id = id });

                 var result = new OrderDetailModel
                 {
                     Id = id,
                     Amount = (decimal)order.Amount,
                     CreateTime = order.CreateTime,
                     Customer = order.UserName,
                     PaymentMethod = order.PaymentMethod,
                     PaymentTime = order.PaymentTime,
                     Promotions = JsonHelper.Deserialize<KeyValuePair<Guid, string>[]>(order.Promotions),
                     State = (Order.OrderState)order.State,
                     Consignee = JsonHelper.Deserialize<Consignee>(order.Consignee),
                 };

                 var items = new List<OrderDetailModel.Item>();
                 var orderItems = con.Query(@"
SELECT ProductName, Specifications, Price, Tax, Promotions, Quantity, State
FROM 'OrderItem'
WHERE OrderId = @Id
", new { Id = id });

                 foreach (var item in orderItems)
                 {
                     items.Add(new OrderDetailModel.Item
                     {
                         Price = (decimal)item.Price,
                         ProductName = item.ProductName,
                         Quantity = (int)item.Quantity,
                         Promotions = JsonHelper.Deserialize<KeyValuePair<Guid, string>[]>(item.Promotions),
                         Specifications = JsonHelper.Deserialize<KeyValuePair<string, string>[]>(item.Specifications),
                         State = (OrderItem.OrderItemState)item.State,
                         Tax = (decimal)item.Tax
                     });
                 }

                 result.Items = items.ToArray();
                 return result;
             });
        }

        private static void CheckStock(Models.Cart.CartModel cart)
        {
            if (cart.Items.Any(a => a.Quantity > a.Stock))
            {
                throw new Exception("We are out of stock, please renew your order");
            }
        }

        public OrderPreviewModel Preview(Guid id)
        {
            var cart = new CartService(Commerce).GetCart(id, filterNotSelected: true);
            CheckStock(cart);

            var order = new OrderPreviewModel
            {
                Amount = cart.DiscountAmount,
                Items = cart.Items.Select(s => new OrderPreviewModel.Item
                {
                    Price = decimal.Round(Helpers.GetCartItemPrice(cart, s.DiscountAmount, s.Quantity), 2),
                    ProductName = s.ProductName,
                    Quantity = s.Quantity,
                    Specifications = s.Specifications
                }).ToArray()
            };

            return order;
        }

        public PagedListModel<OrderListModel> Query(PagingQueryModel model)
        {
            var result = new PagedListModel<OrderListModel>();

            Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                var count = con.Count<Order>();
                result.SetPageInfo(model, count);
                result.List = con.Query<OrderListModel>(@"
SELECT O.Id,
       CreateTime,
       PaymentTime,
       PaymentMethod,
       Amount,
       O.State,
       COUNT(1) AS ItemCount
FROM 'Order' O
         LEFT JOIN OrderItem OI ON O.Id = OI.OrderId
GROUP BY O.Id
LIMIT @Size OFFSET @Offset
", new
                {
                    model.Size,
                    Offset = result.GetOffset()
                }).ToList();
            });

            return result;
        }
    }
}

using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class OrderService : ServiceBase
    {
        public OrderService(RenderContext context) : base(context)
        {
        }

        public void CreateOrder(CreateOrderModel model)
        {
            using (var con = DbConnection)
            {
                con.Open();
                var tran = con.BeginTransaction();
                var cart = new CartService(Context).GetCart(model.CustomerId, con, true);
                CheckStock(cart);

                var order = model.ToOrder(cart);
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
                        SkuId = orderItem.SkuId,
                        StockType = Entities.StockType.Sale
                    });
                }

                con.Insert(order);
                con.InsertList(orderItems);
                con.InsertList(stocks);
                con.DeleteList<Entities.CartItem>(cart.Items.Select(s => s.Id));
                tran.Commit();
            }
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
            var cart = new CartService(Context).GetCart(id, filterNotSelected: true);
            CheckStock(cart);

            var order = new OrderPreviewModel
            {
                Amount = cart.DiscountAmount,
                Items = cart.Items.Select(s => new OrderPreviewModel.OrderItemPreviewModel
                {
                    Price = decimal.Round(Helpers.GetCartItemPrice(cart, s.DiscountAmount, s.Quantity), 2),
                    ProductName = s.ProductName,
                    Quantity = s.Quantity,
                    Specifications = s.Specifications
                }).ToArray()
            };

            return order;
        }
    }
}

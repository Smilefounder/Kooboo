using Kooboo.Sites.Ecommerce.Models;
using System;
using System.Collections.Generic;

namespace Kooboo.Sites.Ecommerce.Service
{
    public interface IOrderService : IEcommerceService<Order>
    {
        bool AddAddress(Guid OrderId, Guid AddressId);
        bool Cancel(Guid orderId);
        int Count();
        Order CreateOrder(Cart cart, Guid addressId);
        Order CreateOrder(List<CartItem> CartItems, Guid addressId);
        bool Finish(Guid orderId);
        List<Order> List(int skip, int take);
        List<Order> ListByCustomerId(int skip, int take);
        bool Paid(Guid orderId);
        bool ReturnStock(Guid id);
        bool UpdateStatus(Guid orderId, OrderStatus status);
    }
}
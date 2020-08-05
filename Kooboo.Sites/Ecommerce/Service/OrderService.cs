using Kooboo.Sites.Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.Service
{
    public class OrderService : ServiceBase<Order>
    {
        //only order some of the shopping cart item... 
        public Order CreateOrder(List<CartItem> CartItems, Guid addressId)
        {
            Cart cart = new Cart();
            cart.Items = CartItems;
            ServiceProvider.Cart(this.Context).CalculatePromotion(cart);
            return CreateOrder(cart, addressId);
        }

        public Order CreateOrder(Cart cart, Guid addressId)
        {
            var shipping = ServiceProvider.Shipping(this.Context).CalculateCost(cart);

            Order neworder = new Order();
            foreach (var item in cart.Items)
            {
                neworder.Items.Add(Mapper.ToOrderLine(item));
            }
            neworder.Discount = cart.Discount;
            neworder.ShippingCost = shipping;
            neworder.AddressId = addressId;
            var address = ServiceProvider.CustomerAddress(Context).Get(addressId);
            neworder.OrderAddress = new OrderAddress(address, neworder.Id);
            neworder.CustomerId = this.CommerceContext.customer.Id;
            neworder.CreateDate = neworder.CreationDate = DateTime.UtcNow;
            this.Repo.AddOrUpdate(neworder);
            return neworder;
        }
        public Order EditOrder(Order neworder)
        {
            var order = this.Repo.Get(neworder.Id);
            if (order != null)
            {
                if (order.OrderAddress != null)
                {
                    order.OrderAddress.Consignee = neworder.OrderAddress.Consignee;
                    order.OrderAddress.ContactNumber = neworder.OrderAddress.ContactNumber;
                    order.OrderAddress.Country = neworder.OrderAddress.Country;
                    order.OrderAddress.City = neworder.OrderAddress.City;
                    order.OrderAddress.PostCode = neworder.OrderAddress.PostCode;
                    order.OrderAddress.HouseNumber = neworder.OrderAddress.HouseNumber;
                    order.OrderAddress.Address = neworder.OrderAddress.Address;
                    order.OrderAddress.Address2 = neworder.OrderAddress.Address2;
                    this.Repo.AddOrUpdate(neworder);
                }
            }
            return neworder;
        }

        public bool AddAddress(Guid OrderId, Guid AddressId)
        {
            var order = this.Repo.Get(OrderId);
            if (order != null)
            {
                this.Repo.Store.UpdateColumn<Guid>(order.Id, o => o.AddressId, AddressId);
                return true;
            }
            return false;
        }

        public bool Paid(Guid orderId)
        {
            // update an order status to paid.
            return UpdateStatus(orderId, OrderStatus.Paid);
        }

        public bool Cancel(Guid orderId)
        {
            return UpdateStatus(orderId, OrderStatus.Cancel);
        }

        public List<Order> ListByCustomerId(int skip, int take)
        {
            var list = this.Repo.Store.Where(it => it.CustomerId == this.CommerceContext.customer.Id).OrderByDescending().Skip(skip).Take(take);
            return list;
        }

        public List<Order> List(int skip, int take)
        {
            var list = this.Repo.Store.Where().OrderByDescending().Skip(skip).Take(take);
            return list;
        }

        public int Count()
        {
            return this.Repo.Store.Count();
        }

        public bool UpdateStatus(Guid orderId, OrderStatus status)
        {
            var order = this.Repo.Get(orderId);
            if (order != null)
            {
                order.Status = status;
                this.Repo.AddOrUpdate(order);
                return true;
            }
            return false;
        }
    }
}

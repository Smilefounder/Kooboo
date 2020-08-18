using Kooboo.Data.Context;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.Service;
using Kooboo.Sites.Ecommerce.ViewModel;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Payment;
using Kooboo.Sites.Payment.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Kooboo.Sites.Ecommerce.KScript
{

    public class KOrder
    {
        public RenderContext context { get; set; }

        public CommerceContext commerceContext { get; set; }

        private IOrderService service { get; set; }

        private ICartService cartService { get; set; }

        public KOrder(RenderContext context)
        {
            this.context = context;
            this.service = ServiceProvider.Order(this.context);
            this.cartService = ServiceProvider.Cart(this.context);
        }

        [Description("Create an order based on current shopping cart items")]
        public Order Create(string addressId)
        {
            Guid id;
            bool parseok = Guid.TryParse(addressId, out id);

            if (parseok)
            {
                var cartservice = ServiceProvider.Cart(this.context);
                var cart = cartservice.GetOrCreateCart();
                if (cart.Items.Any())
                {
                    var order = this.service.CreateOrder(cart, id);

                    // remove cart
                    cartService.Delete(cart.Id);
                    return order;
                }
            }
            return null;
        }

        public Order Get(string orderId)
        {
            if (Guid.TryParse(orderId, out var id))
            {
                return service.Get(id);
            }
            return null;
        }

        public bool SetPaymentRequestId(string orderId, string paymentRequestId)
        {
            if (Guid.TryParse(orderId, out var id) && Guid.TryParse(paymentRequestId, out var paymentRequestIdParsed))
            {
                var order = service.Get(id);
                order.PaymentRequestId = paymentRequestIdParsed;
                this.service.AddOrUpdate(order);
                return true;
            }
            return false;
        }

        public PaymentCallback GetPaymentCallBack(string paymentRequestId)
        {
            if (Guid.TryParse(paymentRequestId, out var paymentRequestIdParsed))
            {
                var sitedb = context.WebSite.SiteDb();
                var callbackrepo = sitedb.GetSiteRepository<PaymentCallBackRepository>();

                return callbackrepo.Query.Where(it => it.RequestId == paymentRequestIdParsed).FirstOrDefault();
            }

            return null;
        }

        public Order CreateSelected(object[] selected, string addressId)
        {
            Guid addressguid;
            bool parseok = Guid.TryParse(addressId, out addressguid);

            if (parseok)
            {
                List<Guid> selectedCartItem = new List<Guid>();
                foreach (var item in selected)
                {
                    if (item != null)
                    {
                        var stritem = item.ToString();

                        if (System.Guid.TryParse(stritem, out Guid id))
                        {
                            selectedCartItem.Add(id);
                        }
                    }
                }

                var cartservice = ServiceProvider.Cart(this.context);
                var cart = cartservice.GetOrCreateCart();
                if (cart.Items.Any() && selectedCartItem.Any())
                {
                    var order = this.service.CreateOrder(cart.Items.FindAll(o => selectedCartItem.Contains(o.Id)).ToList(), addressguid);
                    if (order != null)
                    {
                        foreach (var item in order.Items)
                        {
                            cartservice.RemoveItem(item.ProductVariantId);
                        }
                    }
                    return order;
                }
            }

            return null;
        }

        public void DeleteOrder(object[] OrderIds)
        {
            foreach (var OrderId in OrderIds)
            {
                var guid = Lib.Helper.IDHelper.GetGuid(OrderId);
                if (guid != default(Guid))
                {
                    var order = this.service.Get(guid);
                    if (order != null)
                    {
                        order.DeleteByCustomer = true;

                    }

                    this.service.AddOrUpdate(order);
                }
            }
        }

        public void Cancel(object[] OrderIds)
        {
            foreach (var OrderId in OrderIds)
            {
                var guid = Lib.Helper.IDHelper.GetGuid(OrderId);
                if (guid != default)
                {
                    service.Cancel(guid);
                }
            }
        }

        public void ReturnStock(object[] orderIds)
        {
            foreach (var orderId in orderIds)
            {
                var id = Lib.Helper.IDHelper.GetGuid(orderId);
                if (id != default)
                {
                    service.ReturnStock(id);
                }
            }
        }

        public Order getOrderByPaymentRequestId(object paymentRequestId)
        {
            var guid = Lib.Helper.IDHelper.GetGuid(paymentRequestId);
            if (guid != default)
            {
                return service.GetOrderByPaymentRequestId(guid);
            }
            return null;
        }

        public bool Paid(object orderId)
        {
            var guid = Lib.Helper.IDHelper.GetGuid(orderId);
            if (guid != default)
            {
                return service.Paid(guid);
            }
            return false;
        }

        public bool PaidByPaymentRequestId(object paymentRequestId)
        {
            var order = getOrderByPaymentRequestId(paymentRequestId);
            if(order != null)
            {
                Paid(order.Id);
            }
            return false;
        }

        public int GetTotalCount()
        {
            return this.service.Count();
        }

        public OrderViewModel[] GetOrderList(int skip, int take)
        {
            var orders = this.service.ListByCustomerId(skip, take).Select(it => new OrderViewModel(it, this.context)).ToArray();
            return orders;
        }
    }
}

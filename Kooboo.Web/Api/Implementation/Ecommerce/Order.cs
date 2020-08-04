using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Api;
using Kooboo.Sites.Ecommerce;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.ViewModel;
using Kooboo.Sites.Extensions;
using Kooboo.Web.ViewModel;

namespace Kooboo.Web.Api.Implementation.Ecommerce
{
    public class OrderApi : SiteObjectApi<Order>
    {
        public PagedListViewModel<OrderViewModel> GetOrders(ApiCall call)
        {
            var pager = ApiHelper.GetPager(call, 50);

            var service = Sites.Ecommerce.ServiceProvider.Order(call.Context);

            var items = service.List(pager.SkipCount, pager.PageSize);

            PagedListViewModel<OrderViewModel> model = new PagedListViewModel<OrderViewModel>();

            model.TotalCount = service.Count();
            model.PageNr = pager.PageNr;
            model.PageSize = pager.PageSize;
            model.TotalPages = ApiHelper.GetPageCount(model.TotalCount, pager.PageSize);

            model.List = items.Select(o => new OrderViewModel(o, call.Context)).ToList();

            return model;
        }


        public OrderViewModel GetOrder(ApiCall call, Guid id)
        {
            var service = Sites.Ecommerce.ServiceProvider.Order(call.Context);
            var order = service.Get(id);

            var view = new OrderViewModel(order, call.Context);

            return view;
        }

        public bool ChangeOrderAddress(ApiCall call, Guid orderId, Guid addressId)
        {
            var sitedb = call.WebSite.SiteDb();

            var order = sitedb.Order.Get(orderId);
            if ((int)order.Status < (int)OrderStatus.Shipping)
            {
                order.AddressId = addressId;

                sitedb.Order.AddOrUpdate(order);

                return true;
            }

            return false;
        }

        public bool EditOrderStatus(ApiCall call, List<Guid> orderIds, int status)
        {
            try
            {
                var sitedb = call.WebSite.SiteDb();

                foreach (var orderId in orderIds)
                {
                    var order = sitedb.Order.Get(orderId);
                    order.Status = (OrderStatus)Enum.ToObject(typeof(OrderStatus), status);

                    sitedb.Order.AddOrUpdate(order);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

#if DEBUG
        public CustomerAddress CreateFakeAddress(ApiCall call)
        {
            var address = new CustomerAddress
            {
                CustomerId = new Guid("504d4554-af7e-2fc9-7330-fd46b447e2ec"),
                CreationDate = DateTime.UtcNow,
                Country = "China",
                ContactNumber = "17859520990",
                Address = "福建省",
                Address2 = "厦门市集美区",
                City = "厦门市"
            };
            ServiceProvider.CustomerAddress(call.Context).AddOrUpdate(address);
            return address;
        }
        public Order CreateFakeOrders(ApiCall call)
        {
            var service = ServiceProvider.Order(call.Context);
            var cartItems = new List<CartItem>();
            cartItems.Add(new CartItem
            {
                ProductId = new Guid("f34a7a66-3474-ae7a-bc6d-04d8572fbb85"),
                Discount = null,
                ProductVariantId = new Guid("1a651a3e-9479-a29c-e20e-045689a2cad9"),
                Quantity = 1,
                UnitPrice = 100
            });

            return service.CreateOrder(cartItems, new Guid("b71b0b64-f0c7-4893-a347-7b8fcb0eacfe"));
        }
    }
#endif

}

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

            var service = ServiceProvider.Order(call.Context);

            var items = service.List(pager.SkipCount, pager.PageSize);

            PagedListViewModel<OrderViewModel> model = new PagedListViewModel<OrderViewModel>();

            model.TotalCount = service.Count();
            model.PageNr = pager.PageNr;
            model.PageSize = pager.PageSize;
            model.TotalPages = ApiHelper.GetPageCount(model.TotalCount, pager.PageSize);

            model.List = items.Select(o => new OrderViewModel(o, call.Context)).ToList();

            return model;
        }


        public PagedListViewModel<OrderViewModel> Search(ApiCall call, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetOrders(call);
            }
            var pager = ApiHelper.GetPager(call, 50);
            var sitedb = call.WebSite.SiteDb();

            var items = new List<Order>();
            if (Guid.TryParse(keyword, out var orderId))
            {
                // search by orderid
                items = sitedb.Order.Query.Where(o => o.Id == orderId).SelectAll();
            }
            else if (Enum.TryParse<OrderStatus>(keyword, out var status))
            {
                // search by status
                items = sitedb.Order.Query.Where(o => o.Status == status).SelectAll();
            }
            else if (keyword.Contains("@"))
            {
                // search by user email
                var emailHash = Lib.Security.Hash.ComputeGuidIgnoreCase(keyword);
                var customer = sitedb.Customer.Query.Where(o => o.EmailHash == emailHash).FirstOrDefault();
                if (customer != null)
                {
                    items = sitedb.Order.Query.Where(o => o.CustomerId == customer.Id).SelectAll();
                }
            }
            else
            {
                // serach by telphone
                var telhash = Lib.Security.Hash.ComputeGuidIgnoreCase(keyword);
                var customer = sitedb.Customer.Query.Where(o => o.TelHash == telhash).FirstOrDefault();
                if (customer != null)
                {
                    items = sitedb.Order.Query.Where(o => o.CustomerId == customer.Id).SelectAll();
                }
            }

            PagedListViewModel<OrderViewModel> model = new PagedListViewModel<OrderViewModel>();
            model.TotalCount = items.Count();
            model.PageNr = pager.PageNr;
            model.PageSize = pager.PageSize;
            model.TotalPages = ApiHelper.GetPageCount(model.TotalCount, pager.PageSize);
            model.List = items.OrderByDescending(o => o.CreateDate)
                .Skip(model.PageNr * model.PageSize - model.PageSize)
                .Take(model.PageSize)
                .Select(o => new OrderViewModel(o, call.Context)).ToList();

            return model;
        }


        public OrderViewModel GetOrder(ApiCall call, Guid id)
        {
            var service = ServiceProvider.Order(call.Context);
            var order = service.Get(id);
            var view = new OrderViewModel(order, call.Context);

            return view;
        }

        public OrderViewModel EditOrder(ApiCall call, OrderEditRequestModel neworder)
        {
            var service = ServiceProvider.Order(call.Context);
            var order = service.Get(neworder.Id);
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
                service.AddOrUpdate(order);
            }
            var view = new OrderViewModel(order, call.Context);
            return view;
        }

        public bool Cancel(ApiCall call, Guid id)
        {
            var service = ServiceProvider.Order(call.Context);
            var success = service.Cancel(id);
            return success;
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
                City = "厦门市",
                Consignee = "吴收货"
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
                ProductVariantId = new Guid("d2d63a39-762c-93b0-932b-f79b745aab03"),
                Quantity = 1,
                UnitPrice = 100
            });

            return service.CreateOrder(cartItems, new Guid("5dce97a2-80f2-4cf9-9fd8-fc363de4d866"));
        }
    }
#endif

}

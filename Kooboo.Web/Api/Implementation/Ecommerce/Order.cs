using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Api;
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

            model.TotalCount = items.Count();
            model.PageNr = pager.PageNr;
            model.PageSize = pager.PageSize;
            model.TotalPages = ApiHelper.GetPageCount(model.TotalCount, pager.PageSize);

            model.List = items.Select(o => new OrderViewModel(o, call.Context)).ToList();

            return model;
        }


        public OrderViewModel GetEdit(ApiCall call)
        {
            var service = Sites.Ecommerce.ServiceProvider.Order(call.Context);

            var orderId = call.GetValue<Guid>("id");
            if (orderId != default(Guid))
            {
                var order = service.Get(orderId);
                if (order != null)
                {
                    return new OrderViewModel(order, call.Context);
                }
            }

            return new OrderViewModel(new Order(), call.Context);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Api;
using Kooboo.Sites.Ecommerce;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.ViewModel;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Logistics;
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


        public PagedListViewModel<OrderViewModel> Search(ApiCall call)
        {
            string keyword = call.GetValue("keyword");
            string status = call.GetValue("status");

            var pager = ApiHelper.GetPager(call, 50);
            var sitedb = call.WebSite.SiteDb();

            var items = new List<Order>();

            var query = sitedb.Order.Query;
            if (Enum.TryParse<OrderStatus>(status, out var orderStatus))
            {
                // search by status
                query = query.Where(o => o.Status == orderStatus);
            }

            if (string.IsNullOrWhiteSpace(keyword))
            {
                items = query.SelectAll();
            }
            else
            {
                if (Guid.TryParse(keyword, out var orderId))
                {
                    // search by orderid
                    items = query.Where(o => o.Id == orderId).SelectAll();
                }
                else if (keyword.Contains("@"))
                {
                    // search by user email
                    var emailHash = Lib.Security.Hash.ComputeGuidIgnoreCase(keyword);
                    var customer = sitedb.Customer.Query.Where(o => o.EmailHash == emailHash).FirstOrDefault();
                    if (customer != null)
                    {
                        items = query.Where(o => o.CustomerId == customer.Id).SelectAll();
                    }
                }
                else
                {
                    // serach by telphone
                    var telhash = Lib.Security.Hash.ComputeGuidIgnoreCase(keyword);
                    var customer = sitedb.Customer.Query.Where(o => o.TelHash == telhash).FirstOrDefault();
                    if (customer != null)
                    {
                        items = query.Where(o => o.CustomerId == customer.Id).SelectAll();
                    }
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
            if (order.CustomerAddress != null)
            {
                order.CustomerAddress.Consignee = neworder.CustomerAddress.Consignee;
                order.CustomerAddress.ContactNumber = neworder.CustomerAddress.ContactNumber;
                order.CustomerAddress.Country = neworder.CustomerAddress.Country;
                order.CustomerAddress.City = neworder.CustomerAddress.City;
                order.CustomerAddress.PostCode = neworder.CustomerAddress.PostCode;
                order.CustomerAddress.HouseNumber = neworder.CustomerAddress.HouseNumber;
                order.CustomerAddress.Address = neworder.CustomerAddress.Address;
                order.CustomerAddress.Address2 = neworder.CustomerAddress.Address2;
                order.LogisticsCompany = neworder.logisticsCompany;
                order.LogisticsNumber = neworder.logisticsNumber;
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
        public bool Finish(ApiCall call, Guid id)
        {
            var service = ServiceProvider.Order(call.Context);
            var success = service.Finish(id);
            return success;
        }

        public string GetLogisticsNumber(ApiCall call, Guid id, string logisticsCompany)
        {
            var sender = call.Context.WebSite.SiteDb().CoreSetting.GetSetting<LogisticsSenderSetting>();
            if (sender == null)
            {
                var messge = Data.Language.Hardcoded.GetValue("Please config LogisticsSender first", call.Context);
                throw new Exception(messge);
            }
            var logisticsmethod = LogisticsManager.GetMethod(logisticsCompany, call.Context);
            if (logisticsmethod != null)
            {
                var method = new KLogisticsMethodWrapper(logisticsmethod, call.Context);
                var service = ServiceProvider.Order(call.Context);
                var receiver = service.Get(id).CustomerAddress;
                var info = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "sendername", sender.SenderName },
                    { "senderphone", sender.SenderPhone },
                    { "senderprovince", sender.SenderProvince },
                    { "sendercity", sender.SenderCity },
                    { "sendercounty", sender.SenderCounty },
                    { "senderaddress", sender.SenderAddress },

                    { "receivername", receiver.Consignee },
                    { "receiverphone", receiver.ContactNumber },
                    { "receiverprovince", receiver.City }, // TODO: how to map city to provice ?
                    { "receivercity", receiver.City },
                    { "receiveraddress", receiver.Address + receiver.Address2 }
                };
                var logisticsOrder = method.CreateOrder(info);
                return logisticsOrder?.logisticsMethodReferenceId;
            }
            return null;
        }

        public List<LogisticsCompany> GetAllLogistics(ApiCall call)
        {
            var service = ServiceProvider.Shipping(call.Context);
            return service.GetAllLogistics();
        }

        public bool ChangeCustomerAddress(ApiCall call, Guid orderId, Guid addressId)
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

        public bool Ship(ApiCall call, Guid orderId)
        {
            var sitedb = call.WebSite.SiteDb();
            var order = sitedb.Order.Get(orderId);
            if ((int)order.Status < (int)OrderStatus.Shipping)
            {
                order.Status = OrderStatus.Shipping;
            }
            order.LogisticsCompany = call.GetValue("logisticsCompany");
            order.LogisticsNumber = call.GetValue("logisticsNumber");
            sitedb.Order.AddOrUpdate(order);

            return true;
        }
    }
}

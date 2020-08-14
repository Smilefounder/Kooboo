using Kooboo.Api;
using Kooboo.Data.Language;
using Kooboo.Sites.Ecommerce;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.ViewModel;
using Kooboo.Sites.Extensions;
using Kooboo.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Ecommerce
{
    public class CustomerApi : SiteObjectApi<Customer>
    {
        public PagedListViewModel<CustomerForPageViewModel> CustomerList(ApiCall call)
        {
            var sitedb = call.WebSite.SiteDb();
            int pagesize = ApiHelper.GetPageSize(call, 50);
            int pagenr = ApiHelper.GetPageNr(call);

            PagedListViewModel<CustomerForPageViewModel> model = new PagedListViewModel<CustomerForPageViewModel>();
            model.PageNr = pagenr;
            model.PageSize = pagesize;

            var customers = sitedb.Customer.All();
            model.TotalCount = customers.Count();
            model.TotalPages = ApiHelper.GetPageCount(model.TotalCount, model.PageSize);

            var customerList = customers.OrderByDescending(o => o.LastModified).Skip(model.PageNr * model.PageSize - model.PageSize).Take(model.PageSize).ToList();
            model.List = customerList.Select(it => MapCustomer(call, it)).ToList();

            return model;
        }

        public PagedListViewModel<CustomerForPageViewModel> Search(ApiCall call)
        {
            string keyword = call.GetValue("keyword");
            string status = call.GetValue("status");

            var sitedb = call.WebSite.SiteDb();
            int pagesize = ApiHelper.GetPageSize(call, 50);
            int pagenr = ApiHelper.GetPageNr(call);




            var items = new List<Customer>();

            var query = sitedb.Customer.Query;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                items = query.SelectAll();
            }
            else
            {
                if (keyword.Contains("@"))
                {
                    // search by user email
                    var emailHash = Lib.Security.Hash.ComputeGuidIgnoreCase(keyword);
                    items = query.Where(o => o.EmailHash == emailHash).SelectAll();
                }
                //else
                //{
                //    // serach by telphone
                //    var telhash = Lib.Security.Hash.ComputeGuidIgnoreCase(keyword);
                //    var customer = sitedb.Customer.Query.Where(o => o.TelHash == telhash).FirstOrDefault();
                //    if (customer != null)
                //    {
                //        items = query.Where(o => o.CustomerId == customer.Id).SelectAll();
                //    }
                //}
            }
            PagedListViewModel<CustomerForPageViewModel> model = new PagedListViewModel<CustomerForPageViewModel>();
            model.PageNr = pagenr;
            model.PageSize = pagesize;
            model.TotalCount = items.Count();
            model.TotalPages = ApiHelper.GetPageCount(model.TotalCount, model.PageSize);

            var customerList = items.OrderByDescending(o => o.LastModified).Skip(model.PageNr * model.PageSize - model.PageSize).Take(model.PageSize).ToList();
            model.List = customerList.Select(it => MapCustomer(call, it)).ToList();

            return model;
        }

        public CustomerEditViewModel GetEdit(ApiCall call)
        {
            var sitedb = call.Context.WebSite.SiteDb();

            Customer customer = null;
            var customerViewModel = new CustomerForPageViewModel();

            var customerId = call.GetValue<Guid>("id");
            if (customerId != default(Guid))
            {
                customer = sitedb.Customer.Get(customerId);
                customerViewModel = MapCustomer(call, customer);
            }

            var model = new CustomerEditViewModel();
            model.CustomerViewModel = customerViewModel;

            return model;
        }

        public Guid Post(CustomerUpdateViewModel model, ApiCall call)
        {
            var service = ServiceProvider.Customer(call.Context);

            model.CustomerModel = model.CustomerModel ?? new CustomerModel();

            var sitedb = call.WebSite.SiteDb();

            Customer newCustomer = sitedb.Customer.Get(call.ObjectId);
            if (newCustomer == null)
            {
                var result = service.CreatAccount(model.CustomerModel.UserName, model.CustomerModel.EmailAddress, model.CustomerModel.Password, model.CustomerModel.FirstName, model.CustomerModel.LastName, model.CustomerModel.Telephone);

                return result.Id;
            }
            else
            {
                newCustomer = AddOrUpdateMapping(newCustomer, model);
                sitedb.Customer.AddOrUpdate(newCustomer, call.Context.User.Id);

                return newCustomer.Id;
            }
        }

        public bool IsUniqueName(string userName, ApiCall apiCall)
        {
            var service = ServiceProvider.Customer(apiCall.Context);

            return service.IsUSerNameAvailable(userName);
        }

        public bool IsUniqueEmail(string email, ApiCall apiCall)
        {
            var service = ServiceProvider.Customer(apiCall.Context);

            return service.IsEmailAddressAvailable(email);
        }

        protected static CustomerForPageViewModel MapCustomer(ApiCall call, Customer customer)
        {
            return new CustomerForPageViewModel
            {
                Id = customer.Id,
                UserName = customer.Name,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                Telephone = customer.Telephone,
                MembershipNumber = customer.MembershipNumber
            };
        }

        protected static Customer AddOrUpdateMapping(Customer customer, CustomerUpdateViewModel model)
        {
            customer.Name = model.CustomerModel.UserName;
            customer.FirstName = model.CustomerModel.FirstName;
            customer.LastName = model.CustomerModel.LastName;
            customer.EmailAddress = model.CustomerModel.EmailAddress;
            customer.Telephone = model.CustomerModel.Telephone;
            return customer;
        }
    }
}

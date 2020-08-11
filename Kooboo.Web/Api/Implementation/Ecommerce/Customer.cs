using Kooboo.Api;
using Kooboo.Data.Language;
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

            //sitedb.Customer.AddOrUpdate(new Customer { FirstName = "first name", LastName = "last name", EmailAddress = "tenghui@kooboo.cn", Telephone = "10086" }, call.Context.User.Id);

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

        protected static CustomerForPageViewModel MapCustomer(ApiCall call, Customer customer)
        {
            return new CustomerForPageViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                Telephone = customer.Telephone,
                MembershipNumber = customer.MembershipNumber
            };
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

        public Guid Post(PromotionUpdateViewModel model, ApiCall call)
        {
            //model.PromotionModel = model.PromotionModel ?? new PromotionModel();

            //var sitedb = call.WebSite.SiteDb();
            //var promotionRuleType = Lib.IOC.Service.GetInstances<IPromotionCondition>().FirstOrDefault(it => it.Name == model.PromotionModel.RuleType);

            //var targetValue = new List<string>();
            //if (model.PromotionModel.RuleType == "ByTotalAmount")
            //{
            //    targetValue.Add(model.PromotionModel.PriceAmountReached.ToString());
            //}
            //else if (model.PromotionModel.RuleType == "ByProductCategory")
            //{
            //    targetValue = model.Categories.Select(it => it.ToString()).ToList();
            //}

            //PromotionRule newPromotionRule = sitedb.PromotionRule.Get(call.ObjectId);
            //if (newPromotionRule == null)
            //{
            //    newPromotionRule = new PromotionRule();
            //    newPromotionRule = AddOrUpdateMapping(newPromotionRule, model, promotionRuleType, targetValue);
            //}
            //else
            //{
            //    newPromotionRule = AddOrUpdateMapping(newPromotionRule, model, promotionRuleType, targetValue);
            //}

            //sitedb.PromotionRule.AddOrUpdate(newPromotionRule, call.Context.User.Id);

            //return newPromotionRule.Id;

            return Guid.NewGuid();
        }
    }
}

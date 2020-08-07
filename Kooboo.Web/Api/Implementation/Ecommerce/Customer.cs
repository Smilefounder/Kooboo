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
        public PagedListViewModel<PromotionViewModel> CustomerList(ApiCall call)
        {
            var sitedb = call.WebSite.SiteDb();

            int pagesize = ApiHelper.GetPageSize(call, 50);
            int pagenr = ApiHelper.GetPageNr(call);

            string language = string.IsNullOrEmpty(call.Context.Culture) ? call.WebSite.DefaultCulture : call.Context.Culture;

            PagedListViewModel<PromotionViewModel> model = new PagedListViewModel<PromotionViewModel>();
            model.PageNr = pagenr;
            model.PageSize = pagesize;

            var promotionRules = sitedb.PromotionRule.All();

            model.TotalCount = promotionRules.Count();
            model.TotalPages = ApiHelper.GetPageCount(model.TotalCount, model.PageSize);

            var promotionRuleList = promotionRules.OrderByDescending(o => o.LastModified).Skip(model.PageNr * model.PageSize - model.PageSize).Take(model.PageSize).ToList();

            model.List = new List<PromotionViewModel>();

            model.List = promotionRuleList.Select(it => MapPromotionRule(call, it)).ToList();

            return model;
        }

        protected static PromotionViewModel MapPromotionRule(ApiCall call, PromotionRule promotionRule)
        {
            return new PromotionViewModel
            {
                RuleTypeDisplay = Hardcoded.GetValue(promotionRule.ConditionName ?? "", call.Context),
                RuleType = promotionRule.ConditionName,
                Operator = promotionRule.Operator,
                TargetValue = promotionRule.TargetValue,
                Id = promotionRule.Id,
                Name = promotionRule.PromotionRuleName,
                ForObject = promotionRule.ForObject,
                CanCombine = promotionRule.CanCombine,
                PromotionMethod = promotionRule.PromotionMethod,
                Amount = promotionRule.Amount,
                Percent = promotionRule.Percent,
                StartDate = promotionRule.StartDate.ToString("yyyy-MM-dd HH:mm"),
                EndDate = promotionRule.EndDate.ToString("yyyy-MM-dd HH:mm"),
                IsActive = promotionRule.IsActive,
                ActiveBasedOnDates = promotionRule.ByDate,
                Categories = promotionRule.ConditionName == "ByProductCategory" ? promotionRule.TargetValue : new List<string>(),
                PriceAmountReached = promotionRule.ConditionName == "ByTotalAmount" ? promotionRule.TargetValue.FirstOrDefault() : "0"
            };
        }
    }
}

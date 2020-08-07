using Kooboo.Api;
using Kooboo.Data.Language;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.Promotion;
using Kooboo.Sites.Ecommerce.ViewModel;
using Kooboo.Sites.Extensions;
using Kooboo.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Web.Api.Implementation.Ecommerce
{
    public class PromotionRuleApi : SiteObjectApi<PromotionRule>
    {
        public PagedListViewModel<PromotionViewModel> PromotionList(ApiCall call)
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

        public Guid Post(PromotionUpdateViewModel model, ApiCall call)
        {
            model.PromotionModel = model.PromotionModel ?? new PromotionModel();

            var sitedb = call.WebSite.SiteDb();
            var promotionRuleType = Lib.IOC.Service.GetInstances<IPromotionCondition>().FirstOrDefault(it => it.Name == model.PromotionModel.RuleType);

            var targetValue = new List<string>();
            if (model.PromotionModel.RuleType == "ByTotalAmount")
            {
                targetValue.Add(model.PromotionModel.PriceAmountReached.ToString());
            }
            else if (model.PromotionModel.RuleType == "ByProductCategory")
            {
                targetValue = model.Categories.Select(it => it.ToString()).ToList();
            }

            PromotionRule newPromotionRule = sitedb.PromotionRule.Get(call.ObjectId);
            if (newPromotionRule == null)
            {
                newPromotionRule = new PromotionRule();
                newPromotionRule = AddOrUpdateMapping(newPromotionRule, model, promotionRuleType, targetValue);
            }
            else
            {
                newPromotionRule = AddOrUpdateMapping(newPromotionRule, model, promotionRuleType, targetValue);
            }

            sitedb.PromotionRule.AddOrUpdate(newPromotionRule, call.Context.User.Id);

            return newPromotionRule.Id;
        }

        public PromotionEditViewModel GetEdit(ApiCall call)
        {
            var sitedb = call.Context.WebSite.SiteDb();

            PromotionRule promotionRule = null;
            var promotionViewModel = new PromotionViewModel();

            var promotionId = call.GetValue<Guid>("id");
            if (promotionId != default(Guid))
            {
                promotionRule = sitedb.PromotionRule.Get(promotionId);
                promotionViewModel = MapPromotionRule(call, promotionRule);
            }

            var model = new PromotionEditViewModel();
            model.promotionViewModel = promotionViewModel;

            var promotionRuleTypes = Lib.IOC.Service.GetInstances<IPromotionCondition>();
            model.PromotionRuleTypes = promotionRuleTypes.Select(it => new ItemList
            {
                Text = it.DisplayName(call.Context),
                Value = it.Name
            }).ToList();

            model.PromotionMethods = new List<ItemList>
            {
                new ItemList{ Text =  Hardcoded.GetValue("PromitionByAmount", call.Context), Value = "Amount" },
                new ItemList{ Text =  Hardcoded.GetValue("PromitionByPercent", call.Context), Value = "Percent" }
            };

            var promotionTargetDic = EnumHelper.GetEnumAllNameAndValue<EnumPromotionTarget>();
            model.PromotionTargets = promotionTargetDic.Select(it => new ItemList
            {
                Text = Hardcoded.GetValue(it.Value, call.Context),
                Value = it.Key.ToString()
            }).ToList();

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
                Name = promotionRule.Name,
                ForObject = promotionRule.ForObject,
                CanCombine = promotionRule.CanCombine,
                PromotionMethod = promotionRule.PromotionMethod,
                Amount = promotionRule.Amount,
                Percent = promotionRule.Percent,
                StartDate = promotionRule.StartDate.ToString("yyyy-MM-dd HH:mm"),
                EndDate = promotionRule.EndDate.ToString("yyyy-MM-dd HH:mm"),
                IsActive = promotionRule.IsActive,
                ActiveBasedOnDates = promotionRule.IsActive,
                Categories = promotionRule.ConditionName == "ByProductCategory" ? promotionRule.TargetValue : new List<string>(),
                PriceAmountReached = promotionRule.ConditionName == "ByTotalAmount" ? promotionRule.TargetValue.FirstOrDefault() : "0"
            };
        }

        protected static PromotionRule AddOrUpdateMapping(PromotionRule promotionRule, PromotionUpdateViewModel model, IPromotionCondition promotionRuleType, List<string> targetValue)
        {

            promotionRule.Name = model.PromotionModel.Name;
            promotionRule.ConditionName = model.PromotionModel.RuleType;
            promotionRule.Operator = promotionRuleType != null ? promotionRuleType.AvailableOperators.FirstOrDefault() : "";
            promotionRule.TargetValue = targetValue;
            promotionRule.ForObject = model.PromotionModel.PromotionTarget;
            promotionRule.CanCombine = model.PromotionModel.CanCombine;
            promotionRule.PromotionMethod = model.PromotionModel.PromotionMethod;
            promotionRule.Amount = model.PromotionModel.Amount;
            promotionRule.Percent = model.PromotionModel.Percent;
            promotionRule.StartDate = model.PromotionModel.StartDate ?? DateTime.Now;
            promotionRule.EndDate = model.PromotionModel.EndDate ?? DateTime.Now;
            promotionRule.IsActive = model.PromotionModel.IsActive;
            promotionRule.ByDate = model.PromotionModel.ActiveBasedOnDates;

            return promotionRule;
        }
    }
}

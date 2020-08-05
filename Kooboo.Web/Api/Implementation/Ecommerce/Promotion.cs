using Kooboo.Api;
using Kooboo.Data.Language;
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
        public PagedListViewModel<ProductViewModel> PromotionList(ApiCall call)
        {
            var sitedb = call.WebSite.SiteDb();

            int pagesize = ApiHelper.GetPageSize(call, 50);
            int pagenr = ApiHelper.GetPageNr(call);

            string language = string.IsNullOrEmpty(call.Context.Culture) ? call.WebSite.DefaultCulture : call.Context.Culture;

            PagedListViewModel<ProductViewModel> model = new PagedListViewModel<ProductViewModel>();
            model.PageNr = pagenr;
            model.PageSize = pagesize;

            var products = sitedb.Product.All();

            var promotionRules = sitedb.PromotionRule.All();

            model.TotalCount = products.Count();
            model.TotalPages = ApiHelper.GetPageCount(model.TotalCount, model.PageSize);

            var productlist = products.OrderByDescending(o => o.LastModified).Skip(model.PageNr * model.PageSize - model.PageSize).Take(model.PageSize).ToList();

            model.List = new List<ProductViewModel>();

            foreach (var item in productlist)
            {
                var type = sitedb.ProductType.Get(item.ProductTypeId);

                if (type != null)
                {
                    model.List.Add(new ProductViewModel(item, call.Context, type.Properties));
                }
            }
            return model;
        }

        public override Guid Post(ApiCall call)
        {
            var sitedb = call.WebSite.SiteDb();

            PromotionRule newPromotionRule = sitedb.PromotionRule.Get(call.ObjectId);
            if (newPromotionRule == null)
            {
                newPromotionRule = new PromotionRule { Amount = 10, CanCombine = false, ConditionName = "测试ConditionName2", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10), IsActive = true, Name = "优惠2" };
            }

            sitedb.PromotionRule.AddOrUpdate(newPromotionRule, call.Context.User.Id);

            return newPromotionRule.Id;
        }

        public PromotionEditViewModel GetEdit(ApiCall call)
        {
            var model = new PromotionEditViewModel();

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

            var promotionTargetDic = new Dictionary<int, string>();
            GetEnumAllNameAndValue<EnumPromotionTarget>(ref promotionTargetDic);
            model.PromotionTargets = promotionTargetDic.Select(it => new ItemList
            {
                Text = Hardcoded.GetValue(it.Value, call.Context),
                Value = it.Key.ToString()
            }).ToList();

            return model;
        }


        public void GetEnumAllNameAndValue<T>(ref Dictionary<int, string> dic)
        {
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                dic.Add(Convert.ToInt32(value), value.ToString());
            }
        }
    }
}

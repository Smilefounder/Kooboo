using Dapper;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Models.Promotion;
using Kooboo.Sites.Commerce.Services;
using System.Linq;
using static Kooboo.Sites.Commerce.Entities.Promotion;

namespace Kooboo.Sites.Commerce.Cache
{
    public class PromotionCache : CacheBase<PromotionMatchModel[]>
    {
        public PromotionCache(SiteCommerce commerce) : base(commerce)
        {
            var promotionService = commerce.Service<PromotionService>();
            promotionService.OnChanged += _ => Clear();
            promotionService.OnDeleted += _ => Clear();
        }

        protected override PromotionMatchModel[] OnGet()
        {
            return Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                var list = con.Query(@"
SELECT Id,
       Name,
       Type,
       Priority,
       Exclusive,
       Discount,
       Rules,
       Target,
       StartTime
FROM Promotion
WHERE CURRENT_TIMESTAMP < DATETIME(Promotion.EndTime)
  AND Enable = 1
ORDER BY Exclusive DESC, Priority DESC
");

                return list.Select(s => new PromotionMatchModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Type = (PromotionType)s.Type,
                    Priority = (int)s.Priority,
                    Exclusive = s.Exclusive,
                    Discount = (decimal)s.Discount,
                    Rules = JsonHelper.Deserialize<PromotionModel.PromotionRules>(s.Rules),
                    Target = (PromotionTarget)s.Target,
                    StartTime = s.StartTime
                }).ToArray();
            });
        }
    }
}

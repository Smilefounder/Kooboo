using Dapper;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.MatchRule.TargetModels;
using Kooboo.Sites.Commerce.Models.Category;
using Kooboo.Sites.Commerce.Models.ProductCategory;
using Kooboo.Sites.Commerce.Models.Promotion;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Kooboo.Sites.Commerce.Entities.Promotion;

namespace Kooboo.Sites.Commerce.Cache
{
    public class SiteCache
    {
        readonly ConcurrentDictionary<string, CacheItem<dynamic>> _caches = new ConcurrentDictionary<string, CacheItem<dynamic>>();
        public int? LastMigrateVersion { get; set; }
        public bool IsMigrated => LastMigrateVersion != null;

        public IEnumerable<MatchRule.TargetModels.Product> GetMatchProducts(RenderContext context)
        {
            return _caches.GetOrAdd(nameof(GetMatchProducts), _ =>
            {
                var data = context.CreateCommerceDbConnection()
                                .ExecuteTask(con =>
                                {
                                    return con.Query<MatchRule.TargetModels.Product>(@"
SELECT P.Id,
       P.Title,
       P.TypeId,
       PS.Price,
       PS.Tax,
       CASE WHEN P.Enable=1 THEN PS.Enable ELSE 0 END AS Enable
FROM ProductSku PS
         LEFT JOIN Product P ON P.Id = PS.ProductId
");
                                });

                return new CacheItem<dynamic>
                {
                    Data = data,
                    Lifetime = TimeSpan.FromDays(30)
                };
            }).Data;
        }

        public void ClearMatchProducts()
        {
            _caches.TryRemove(nameof(GetMatchProducts), out _);
            ClearProductCategories();
        }

        public IEnumerable<ProductCategoryModel> GetProductCategories(RenderContext context)
        {
            return _caches.GetOrAdd(nameof(GetProductCategories), _ =>
            {
                var data = context.CreateCommerceDbConnection()
                                .ExecuteTask(con => con.Query<ProductCategoryModel>(@"
SELECT PC.ProductId, PC.CategoryId,P.Enable AS ProductEnable,C.Enable AS CategoryEnable
FROM ProductCategory PC
         LEFT JOIN Product P ON P.Id = PC.ProductId
         LEFT JOIN Category C ON C.Id = PC.CategoryId
"));

                return new CacheItem<dynamic>
                {
                    Data = data,
                    Lifetime = TimeSpan.FromDays(30)
                };
            }).Data;
        }

        public void ClearProductCategories()
        {
            _caches.TryRemove(nameof(GetProductCategories), out _);
        }

        public IEnumerable<CategoryModel> GetCategories(RenderContext context)
        {
            return _caches.GetOrAdd(nameof(GetCategories), _ =>
            {
                var data = context.CreateCommerceDbConnection().ExecuteTask(con =>
                {
                    return con.GetList<Category>().Select(s => new CategoryModel(s));
                });

                return new CacheItem<dynamic>
                {
                    Data = data,
                    Lifetime = TimeSpan.FromDays(30)
                };
            }).Data;
        }

        public void ClearCategories()
        {
            _caches.TryRemove(nameof(GetCategories), out _);
            ClearProductCategories();
        }

        public IEnumerable<PromotionMatchModel> GetPromotions(RenderContext context)
        {
            var now = DateTime.UtcNow;

            IEnumerable<PromotionMatchModel> result = _caches.GetOrAdd(nameof(GetPromotions), _ =>
             {
                 var data = context.CreateCommerceDbConnection().ExecuteTask(con =>
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
                     });
                 });


                 return new CacheItem<dynamic>
                 {
                     Data = data.ToArray(),
                     Lifetime = TimeSpan.FromDays(30)
                 };
             }).Data;

            return result.Where(w => w.StartTime < now);
        }

        public void ClearPromotions()
        {
            _caches.TryRemove(nameof(GetPromotions), out _);
        }
    }
}


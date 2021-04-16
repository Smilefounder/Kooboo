using Dapper;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.MatchRule.TargetModels;
using Kooboo.Sites.Commerce.Models.ProductCategory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Cache
{
    public class SiteCache
    {
        readonly ConcurrentDictionary<string, CacheItem<object>> _caches = new ConcurrentDictionary<string, CacheItem<object>>();
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

                return new CacheItem<object>
                {
                    Data = data,
                    Lifetime = TimeSpan.FromDays(30)
                };
            }).Data as IEnumerable<MatchRule.TargetModels.Product>;
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

                return new CacheItem<object>
                {
                    Data = data,
                    Lifetime = TimeSpan.FromDays(30)
                };
            }).Data as IEnumerable<ProductCategoryModel>;
        }

        public void ClearProductCategories()
        {
            _caches.TryRemove(nameof(GetProductCategories), out _);
        }

        public IEnumerable<Category> GetCategories(RenderContext context)
        {
            return _caches.GetOrAdd(nameof(GetCategories), _ =>
            {
                var data = context.CreateCommerceDbConnection().ExecuteTask(con => con.GetList<Category>());

                return new CacheItem<object>
                {
                    Data = data,
                    Lifetime = TimeSpan.FromDays(30)
                };
            }).Data as IEnumerable<Category>;
        }

        public void ClearCategories()
        {
            _caches.TryRemove(nameof(GetCategories), out _);
            ClearProductCategories();
        }
    }
}


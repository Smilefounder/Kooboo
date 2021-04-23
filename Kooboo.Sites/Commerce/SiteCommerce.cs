using Dapper;
using Kooboo.Data;
using Kooboo.Data.Context;
using Kooboo.Data.Events;
using Kooboo.Data.Models;
using Kooboo.Events;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Cache;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Migration;
using Kooboo.Sites.Commerce.Models.Category;
using Kooboo.Sites.Commerce.Models.ProductCategory;
using Kooboo.Sites.Commerce.Models.Promotion;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using static Kooboo.Sites.Commerce.Entities.Promotion;

namespace Kooboo.Sites.Commerce
{
    public class SiteCommerce
    {
        static readonly ConcurrentDictionary<WebSite, SiteCommerce> _siteCache = new ConcurrentDictionary<WebSite, SiteCommerce>();
        readonly ConcurrentDictionary<string, CacheItem<dynamic>> _caches = new ConcurrentDictionary<string, CacheItem<dynamic>>();
        readonly string _connectionString;
        readonly Dictionary<Type, ServiceBase> _serviceMap;
        readonly Dictionary<Type, dynamic> _cacheMap;


        public static SiteCommerce Get(WebSite webSite)
        {
            return _siteCache.GetOrAdd(webSite, _ =>
            {
                return new SiteCommerce(webSite);
            });
        }

        public static void Remove(WebSite webSite)
        {
            _siteCache.TryRemove(webSite, out _);
        }

        public int LastMigrateVersion { get; }

        public SiteCommerce(WebSite site)
        {
            _serviceMap = Lib.IOC.Service.GetImplementationTypes<ServiceBase>().ToDictionary(k => k, v =>
            {
                return Activator.CreateInstance(v, new[] { this }) as ServiceBase;
            });

            _cacheMap = Lib.IOC.Service.GetImplementationTypes(typeof(ICacheBase)).ToDictionary(k => k, v =>
            {
                return Activator.CreateInstance(v, new[] { this });
            });

            var path = Path.Combine(AppSettings.GetFileIORoot(site), "commerce.db");
            _connectionString = $"Data source = '{path}';Version=3;BinaryGUID=False;foreign keys=true";
            LastMigrateVersion = Migrator.Migrate(CreateDbConnection());
        }

        public T Service<T>() where T : ServiceBase
        {
            return _serviceMap[typeof(T)] as T;
        }

        public T Cache<T>() where T : class, ICacheBase
        {
            return _cacheMap[typeof(T)] as T;
        }

        public IDbConnection CreateDbConnection() => new SQLiteConnection(_connectionString);

        public IEnumerable<MatchRule.TargetModels.Product> GetMatchProducts()
        {
            return _caches.GetOrAdd(nameof(GetMatchProducts), _ =>
            {
                var data = CreateDbConnection()
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

        public IEnumerable<ProductCategoryModel> GetProductCategories()
        {
            return _caches.GetOrAdd(nameof(GetProductCategories), _ =>
            {
                var data = CreateDbConnection()
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

        public IEnumerable<CategoryModel> GetCategories()
        {
            return _caches.GetOrAdd(nameof(GetCategories), _ =>
            {
                var data = CreateDbConnection().ExecuteTask(con =>
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

        public IEnumerable<PromotionMatchModel> GetPromotions()
        {
            var now = DateTime.UtcNow;

            IEnumerable<PromotionMatchModel> result = _caches.GetOrAdd(nameof(GetPromotions), _ =>
             {
                 var data = CreateDbConnection().ExecuteTask(con =>
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

    class CommerceCacheWebSiteChangeHandler : IHandler<WebSiteChange>
    {
        public void Handle(WebSiteChange theEvent, RenderContext context)
        {
            SiteCommerce.Remove(theEvent.WebSite);
        }
    }
}


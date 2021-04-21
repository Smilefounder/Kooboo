using Kooboo.Data.Context;
using Kooboo.Data.Events;
using Kooboo.Data.Models;
using Kooboo.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Cache
{
    public static class CommerceCache
    {
        static readonly ConcurrentDictionary<WebSite, SiteCache> _siteCache = new ConcurrentDictionary<WebSite, SiteCache>();

        public static SiteCache GetCache(RenderContext context)
        {
            var site = _siteCache.GetOrAdd(context.WebSite, _ =>
            {
                return new SiteCache();
            });

            return site;
        }

        public static void RemoveCache(WebSite webSite)
        {
            _siteCache.TryRemove(webSite, out _);
        }
    }

    class CommerceCacheWebSiteChangeHandler : IHandler<WebSiteChange>
    {
        public void Handle(WebSiteChange theEvent, RenderContext context)
        {
            CommerceCache.RemoveCache(theEvent.WebSite);
        }
    }
}

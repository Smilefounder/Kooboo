using Kooboo.Data.Context;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Cache
{
    public static class CommerceCache
    {
        static ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, SiteCache>> _globalCache = new ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, SiteCache>>();

        static CommerceCache()
        {
            // TODO auto clean
        }

        public static SiteCache GetCache(RenderContext context)
        {
            var org = _globalCache.GetOrAdd(context.WebSite.OrganizationId, _ => new ConcurrentDictionary<Guid, SiteCache>());
            return org.GetOrAdd(context.WebSite.Id, _ => new SiteCache());
        }
    }
}

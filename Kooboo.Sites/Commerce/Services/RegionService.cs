using Kooboo.Sites.Commerce.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Kooboo.Sites.Commerce.Services
{
    public class RegionService : ServiceBase
    {
        public RegionService(SiteCommerce commerce) : base(commerce)
        {
        }

        public object[] GetCountries()
        {
            var cache = Commerce.Cache<RegionCache>().Data;

            return cache.Select(s => new
            {
                name = s.Value<string>("name"),
                code = s.Value<string>("code"),
            }).ToArray();
        }

        public object[] GetProvinces(string code)
        {
            throw new NotImplementedException();
        }
    }
}

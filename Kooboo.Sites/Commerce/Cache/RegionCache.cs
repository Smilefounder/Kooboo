using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Kooboo.Sites.Commerce.Cache
{
    public class RegionCache : CacheBase<JArray>
    {
        public RegionCache(SiteCommerce commerce) : base(commerce)
        {
        }

        protected override JArray OnGet()
        {
            using (var stream = Assembly.GetAssembly(GetType()).GetManifestResourceStream("Kooboo.Sites.Commerce.countries.json"))
            {
                var serializer = new JsonSerializer();

                using (var sr = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(sr))
                    {
                        return serializer.Deserialize<JArray>(jsonTextReader);
                    }
                }
            }
        }
    }
}

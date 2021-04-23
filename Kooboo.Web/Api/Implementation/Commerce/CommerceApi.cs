using Kooboo.Api;
using Kooboo.Sites.Commerce;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public abstract class CommerceApi : IApi
    {
        public abstract string ModelName { get; }

        public bool RequireSite => true;

        public bool RequireUser => false;

        public T GetService<T>(ApiCall apiCall) where T : ServiceBase
        {
            var commerce = SiteCommerce.Get(apiCall.WebSite);
            return commerce.Service<T>();
        }
    }
}

using Kooboo.Data.Context;
using System;

namespace Kooboo.Sites.Commerce
{
    public class ServiceBase
    {
        public SiteCommerce Commerce { get; }

        public event Action<Guid[]> OnChanged;
        public event Action<Guid[]> OnDeleted;

        public ServiceBase(SiteCommerce commerce)
        {
            Commerce = commerce;
        }

        protected void Changed(params Guid[] ids) => OnChanged?.Invoke(ids);
        protected void Deleted(params Guid[] ids) => OnDeleted?.Invoke(ids);
    }
}

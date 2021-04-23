using Kooboo.Data.Context;
using System;

namespace Kooboo.Sites.Commerce
{
    public class ServiceBase
    {
        public SiteCommerce Commerce { get; }

        public event Action<Guid> OnChanged;
        public event Action<Guid[]> OnDeleted;

        public ServiceBase(SiteCommerce commerce)
        {
            Commerce = commerce;
        }

        protected void Changed(Guid id) => OnChanged?.Invoke(id);
        protected void Deleted(Guid[] ids) => OnDeleted?.Invoke(ids);
    }
}

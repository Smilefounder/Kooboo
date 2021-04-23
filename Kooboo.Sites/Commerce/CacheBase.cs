using System;

namespace Kooboo.Sites.Commerce
{
    public interface ICacheBase
    {
        void Clear();
    }

    public abstract class CacheBase<T> : ICacheBase
    {
        Lazy<T> _data;
        protected SiteCommerce Commerce { get; }

        protected CacheBase(SiteCommerce commerce)
        {
            Clear();
            Commerce = commerce;
        }

        public T Data => _data.Value;

        protected abstract T OnGet();

        public void Clear()
        {
            _data = new Lazy<T>(OnGet, true);
        }
    }
}

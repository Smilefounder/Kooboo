using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Cache
{
    public class CacheItem<T>
    {
        public T Data { get; set; }
        public DateTime CreateTime { get; private set; } = DateTime.UtcNow;
        public TimeSpan? Lifetime { get; set; }
    }
}

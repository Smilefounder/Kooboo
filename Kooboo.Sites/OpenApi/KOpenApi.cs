using Kooboo.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.OpenApi
{
    [KValueType(typeof(object))]
    public class KOpenApi
    {

        [KIgnore]
        public object this[string key]
        {
            get
            {
                return Get(key);
            }
        }

        public object Get(string name)
        {
            return null;
        }
    }
}

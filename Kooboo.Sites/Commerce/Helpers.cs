using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce
{
    public static class Helpers
    {
        public static object GetEnumDescription(Type type)
        {
            return Enum.GetNames(type).Select(s => new
            {
                Name = s,
                Display = s
            });
        }
    }
}

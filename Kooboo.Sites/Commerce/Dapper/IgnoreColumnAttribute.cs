using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreColumnAttribute : Attribute
    {
    }
}

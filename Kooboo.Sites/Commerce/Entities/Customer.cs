using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Customer: EntityBase
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}

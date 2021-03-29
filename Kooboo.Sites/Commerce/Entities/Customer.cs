using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Customer : EntityBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

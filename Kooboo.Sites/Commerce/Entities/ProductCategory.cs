using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class ProductCategory : EntityBase
    {
        public string Name { get; set; }
        public Guid? Parent { get; set; }
    }
}

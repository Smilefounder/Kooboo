using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class ProductType : EntityBase
    {
        public string Name { get; set; }
        public string Attributes { get; set; }
        public string Specifications { get; set; }
    }
}

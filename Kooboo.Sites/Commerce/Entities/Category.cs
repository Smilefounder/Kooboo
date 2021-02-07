using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public Guid? Parent { get; set; }
        public string Properties { get; set; }
        public string Specifications { get; set; }
    }
}

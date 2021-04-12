using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Product : EntityBase
    {
        public string Title { get; set; }
        public string Images { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }
        public string Specifications { get; set; }
        public Guid TypeId { get; set; }
        public bool Enable { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

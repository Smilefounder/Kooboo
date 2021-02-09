using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Sku : EntityBase
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Specifications { get; set; }
        public string Thumbnail { get; set; }
    }
}

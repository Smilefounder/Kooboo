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
        public string Properties { get; set; }
        public Guid CategoryId { get; set; }
    }
}

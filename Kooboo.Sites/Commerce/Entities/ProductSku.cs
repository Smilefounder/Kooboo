using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class ProductSku : EntityBase
    {
        public string Name { get; set; }

        public string Properties { get; set; }

        public Guid ProductId { get; set; }
    }
}

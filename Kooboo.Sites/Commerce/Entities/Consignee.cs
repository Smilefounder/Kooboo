using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Consignee : EntityBase
    {
        public Guid CustomerId { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}

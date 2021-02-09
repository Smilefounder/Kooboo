using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Order:EntityBase
    {
        public DateTime CreateDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}

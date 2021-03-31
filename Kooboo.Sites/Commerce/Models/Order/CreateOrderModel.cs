using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Order
{
    public class CreateOrderModel
    {
        public Guid CustomerId { get; set; }
        public OrderAddress Address { get; set; }
        public string PaymentMethod { get; set; }

        public class OrderAddress
        {
            public string State { get; set; }
            public string Province { get; set; }
            public string City { get; set; }
            public string County { get; set; }
            public string Detail { get; set; }
        }
    }
}

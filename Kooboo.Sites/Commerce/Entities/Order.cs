using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Order : EntityBase
    {
        public Guid CustomerId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime PaymentTime { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public OrderState State { get; set; }

        public string Promotions { get; set; }

        public enum OrderState
        {
            WaitingPay = 0,
            Paid = 1,
            Shipped = 2,
            Finish = 3,
            Cancel = 4
        }
    }
}

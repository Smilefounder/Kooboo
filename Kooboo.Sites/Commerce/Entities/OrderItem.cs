using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class OrderItem : EntityBase
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductVariantId { get; set; }
        public string ProductName { get; set; }
        public string Specifications { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public string Promotions { get; set; }
        public int Quantity { get; set; }
        public OrderItemState State { get; set; }

        public enum OrderItemState
        {
            WaitingPay = 0,
            Paid = 1,
            Shipped = 2,
            Finish = 3,
            Refunding = 4,
            Refunded = 5
        }
    }
}

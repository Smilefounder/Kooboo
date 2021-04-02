using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Models.Cart;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Order
{
    public class CreateOrderModel
    {
        public Guid CustomerId { get; set; }
        public Guid ConsigneeId { get; set; }
        public string PaymentMethod { get; set; }

        public Entities.Order ToOrder(CartModel cart)
        {
            return new Entities.Order
            {
                Id = Guid.NewGuid(),
                Amount = cart.DiscountAmount,
                CreateTime = DateTime.UtcNow,
                CustomerId = CustomerId,
                PaymentMethod = PaymentMethod,
                Promotions = JsonHelper.Serialize(cart.Promotions),
                State = Entities.Order.OrderState.WaitingPay
            };
        }
    }
}

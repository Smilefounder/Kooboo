using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using static Kooboo.Sites.Commerce.Entities.Order;

namespace Kooboo.Sites.Commerce.Models.Order
{
    public class OrderListModel
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime PaymentTime { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderState State { get; set; }
        public int ItemCount { get; set; }
    }
}

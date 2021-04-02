using Kooboo.Sites.Commerce.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using static Kooboo.Sites.Commerce.Entities.Order;
using static Kooboo.Sites.Commerce.Entities.OrderItem;

namespace Kooboo.Sites.Commerce.Models.Order
{
    public class OrderDetailModel
    {
        public Guid Id { get; set; }
        public string Customer { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OrderState State { get; set; }
        public KeyValuePair<Guid, string>[] Promotions { get; set; }
        public Consignee Consignee { get; set; }

        public Item[] Items { get; set; }

        public class Item
        {
            public string ProductName { get; set; }
            public KeyValuePair<string, string>[] Specifications { get; set; }
            public decimal Price { get; set; }
            public decimal Tax { get; set; }
            public KeyValuePair<Guid, string>[] Promotions { get; set; }
            public int Quantity { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public OrderItemState State { get; set; }
        }
    }
}

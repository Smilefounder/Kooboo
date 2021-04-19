using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using static Kooboo.Sites.Commerce.Entities.Promotion;

namespace Kooboo.Sites.Commerce.Models.Promotion
{
    public class PromotionListModel
    {
        private string _discount;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
        public bool Exclusive { get; set; }
        public int Priority { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PromotionTarget Target { get; set; }
        public PromotionType Type { get; set; }
        public string Discount
        {
            get
            {
                return Type == 0 ? $"-{_discount}" : $"-{_discount}%";
            }
            set => _discount = value;
        }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}

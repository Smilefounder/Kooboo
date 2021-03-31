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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Exclusive { get; set; }
        public int Priority { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PromotionTarget Target { get; set; }
        public string Discount { get; set; }
    }
}

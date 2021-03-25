using Kooboo.Lib.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels.Promotion
{
    public class PromotionViewModel
    {
        public PromotionViewModel()
        {

        }


        public PromotionViewModel(Entities.Promotion promotion)
        {
            Id = promotion.Id;
            Name = promotion.Name;
            Description = promotion.Description;
            Type = promotion.Type;
            Priority = promotion.Priority;
            Exclusive = promotion.Exclusive;
            Discount = promotion.Discount;
            Rules = JsonHelper.Deserialize<PromotionRules>(promotion.Rules);
            Target = promotion.Target;
            StartTime = promotion.StartTime;
            EndTime = promotion.EndTime;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Entities.Promotion.PromotionType Type { get; set; }
        public int Priority { get; set; }
        public bool Exclusive { get; set; }
        public decimal Discount { get; set; }
        public PromotionRules Rules { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Entities.Promotion.PromotionTarget Target { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public class PromotionRules
        {
            public MatchRule.Rule Order { get; set; }
            public MatchRule.Rule OrderItem { get; set; }
        }

        public Entities.Promotion ToPromotion()
        {
            return new Entities.Promotion
            {
                Name = Name,
                Description = Description,
                EndTime = EndTime.ToUniversalTime(),
                Exclusive = Exclusive,
                Id = Id,
                Priority = Priority,
                Rules = JsonHelper.Serialize(Rules),
                StartTime = StartTime.ToUniversalTime(),
                Target = Target,
                Type = Type,
                Discount = Discount
            };
        }
    }
}

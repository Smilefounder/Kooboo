using Kooboo.Sites.Ecommerce.Promotion;
using System;
using System.Collections.Generic;

namespace Kooboo.Sites.Ecommerce.ViewModel
{
    public class PromotionViewModel
    {
        public string RuleType { get; set; }

        public string RuleTypeDisplay { get; set; }

        public string Operator { get; set; }

        public List<string> TargetValue { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public EnumPromotionTarget ForObject { get; set; }

        public bool CanCombine { get; set; }

        public EnumPromotionMethod PromotionMethod { get; set; }

        public decimal Amount { get; set; }

        public decimal Percent { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public bool IsActive { get; set; }

        public bool ActiveBasedOnDates { get; set; }

        public List<string> Categories { get; set; }

        public string PriceAmountReached { get; set; }
    }
}

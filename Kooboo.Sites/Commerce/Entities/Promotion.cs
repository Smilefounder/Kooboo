using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Promotion : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public PromotionType Type { get; set; }

        /// <summary>
        /// If Exclusive is true
        /// Promotion1: priority 1 , Exclusive true
        /// Promotion2: priority 2 , Exclusive true
        /// Promotion2 will apply and ignore Promotion1
        /// </summary>
        public int Priority { get; set; }

        public bool Exclusive { get; set; }

        /// <summary>
        /// amount=200
        /// for percentOff 75 =>amount-amount*75% 200*0.25=50
        /// for MoneyOff 20 =>amount-20=150
        /// </summary>
        public decimal Discount { get; set; }

        public string Rules { get; set; }

        public PromotionTarget Target { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool Enable { get; set; }

        public enum PromotionType
        {
            MoneyOff = 0,
            PercentOff = 1
        }

        public enum PromotionTarget
        {
            Order = 0,
            OrderItem = 1
        }
    }
}

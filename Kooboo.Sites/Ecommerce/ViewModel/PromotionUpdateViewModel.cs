//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Sites.Ecommerce.ViewModel
{
    public class PromotionUpdateViewModel
    {
        public List<Guid> Categories { get; set; }

        public string Id { get; set; }

        public string UserKey { get; set; }

        //public string ProductTypeId { get; set; }

        public bool Online { get; set; }

        public PromotionModel PromotionModel { get; set; }
    }

    public class PromotionModel
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool CanCombine { get; set; }

        public bool ActiveBasedOnDates { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string RuleType { get; set; }

        public EnumPromotionMethod PromotionMethod { get; set; }

        public string PriceAmountReached { get; set; }

        public decimal Amount { get; set; }

        public decimal Percent { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.ViewModel
{
    public class PromotionEditViewModel
    {
        public List<ItemList> PromotionRuleTypes { get; set; }

        public List<ItemList> PromotionMethods { get; set; }

        public List<ItemList> PromotionTargets { get; set; }
    }

    public class ItemList
    {
        public string Text { get; set; }

        public string Value { get; set; }
    }
}

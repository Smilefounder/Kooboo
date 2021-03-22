using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.Category
{
    public class ProductRuleModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Guid TypeId { get; set; }
    }
}

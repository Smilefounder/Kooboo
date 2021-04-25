using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.TargetModels
{
    public class Product : TargetModelBase<Product>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public Guid TypeId { get; set; }
    }
}

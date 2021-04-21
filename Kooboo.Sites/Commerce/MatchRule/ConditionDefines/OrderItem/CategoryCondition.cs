using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.ConditionDefines.OrderItem
{
    public class CategoryCondition : ConditionDefineBase<TargetModels.OrderItem>
    {
        public override string Name => "Category";

        public override ConditionValueType ValueType => ConditionValueType.CategoryId;

        public override Comparer[] Comparers => new[] { Comparer.Contains, Comparer.NotContains };

        protected override object GetPropertyValue(TargetModels.OrderItem model)
        {
            return model.Categories;
        }
    }
}

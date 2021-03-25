using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.ConditionDefines.OrderItem
{
    public class ProductCondition : ConditionDefineBase<TargetModels.OrderItem>
    {
        public override string Name => "Product";

        public override ConditionValueType ValueType => ConditionValueType.ProductId;

        protected override object GetPropertyValue(TargetModels.OrderItem model)
        {
            return model.ProductId;
        }
    }
}

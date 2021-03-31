using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.ConditionDefines.OrderItem
{
    public class QuantityCondition : ConditionDefineBase<TargetModels.OrderItem>
    {
        public override string Name => "Quantity";

        public override ConditionValueType ValueType => ConditionValueType.Number;

        protected override object GetPropertyValue(TargetModels.OrderItem model)
        {
            return model.Quantity;
        }
    }
}

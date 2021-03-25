using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.ConditionDefines.Order
{
    public class AmountCondition : ConditionDefineBase<TargetModels.Order>
    {
        public override string Name => "Amount";

        public override ConditionValueType ValueType => ConditionValueType.Number;

        protected override object GetPropertyValue(TargetModels.Order model)
        {
            return model.Amount;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.Category
{
    public class PriceCondition : ConditionDefine<TargetModels.Product>
    {
        public override string Name => "Price";

        public override ConditionValueType ValueType => ConditionValueType.Number;

        protected override object GetPropertyValue(TargetModels.Product model)
        {
            return model.Price;
        }
    }
}

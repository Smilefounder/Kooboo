using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.ConditionDefines.Product
{
    public class TaxCondition : ConditionDefineBase<TargetModels.Product>
    {
        public override string Name => "Tax";

        public override ConditionValueType ValueType => ConditionValueType.Number;

        protected override object GetPropertyValue(TargetModels.Product model)
        {
            return model.Tax;
        }
    }
}

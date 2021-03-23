using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.Category
{
    public class TypeCondition : ConditionDefine<TargetModels.Product>
    {
        public override string Name => "ProductType";

        public override ConditionValueType ValueType => ConditionValueType.ProductTypeId;

        protected override object GetPropertyValue(TargetModels.Product model)
        {
            return model.TypeId;
        }
    }
}

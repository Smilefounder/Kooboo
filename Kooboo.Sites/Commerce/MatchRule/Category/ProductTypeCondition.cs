using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.Category
{
    public class ProductTypeCondition : ConditionDefine<ProductRuleModel>
    {
        public override string Name => "ProductType";

        public override ConditionValueType ValueType => ConditionValueType.ProductTypeId;

        protected override object GetPropertyValue(ProductRuleModel model)
        {
            return model.TypeId;
        }
    }
}

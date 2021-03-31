using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.ConditionDefines.Product
{
    public class TitleCondition : ConditionDefineBase<TargetModels.Product>
    {
        public override string Name => "Title";

        public override ConditionValueType ValueType => ConditionValueType.String;

        protected override object GetPropertyValue(TargetModels.Product model)
        {
            return model.Title;
        }
    }
}

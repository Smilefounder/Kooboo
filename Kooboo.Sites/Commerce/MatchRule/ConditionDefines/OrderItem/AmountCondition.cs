﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule.ConditionDefines.OrderItem
{
    public class AmountCondition : ConditionDefineBase<TargetModels.OrderItem>
    {
        public override string Name => "TotalAmount";

        public override ConditionValueType ValueType => ConditionValueType.Number;

        protected override object GetPropertyValue(TargetModels.OrderItem model)
        {
            return model.Amount;
        }
    }
}
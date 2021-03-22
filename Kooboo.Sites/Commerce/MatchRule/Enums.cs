using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule
{
    public enum Comparer
    {
        EqualTo = 0,
        GreaterThan = 1,
        GreaterThanOrEqual = 2,
        LessThan = 3,
        LessThanOrEqual = 4,
        NotEqualTo = 5,
        StartWith = 6,
        Contains = 7,
    }

    public enum MatchingType
    {
        All = 0,
        Any = 1
    }

    public enum ConditionValueType
    {
        String = 0,
        Number = 1,
        Boolean = 2,
        Datetime = 3,
        ProductId = 4,
        ProductTypeId = 5
    }
}

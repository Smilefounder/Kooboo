using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Expressions
{
    public abstract class DmlExpression
    {
        public abstract DmlExpressionType NodeType { get; }
    }
}

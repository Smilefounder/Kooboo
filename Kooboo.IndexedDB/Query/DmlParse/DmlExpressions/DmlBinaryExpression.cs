using ConditionParser.Modes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Expressions
{
    public class DmlBinaryExpression : DmlExpression
    {
        public DmlExpression Left { get; set; }
        public Operand Operand { get; set; }
        public DmlExpression Right { get; set; }

        public override DmlExpressionType NodeType => DmlExpressionType.Binary;
    }
}

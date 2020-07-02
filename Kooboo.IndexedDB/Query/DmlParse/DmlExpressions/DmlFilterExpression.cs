using ConditionParser.Modes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Expressions
{
    public class DmlFilterExpression : DmlExpression
    {
        public string Property { get; set; }//a
        public Comparer Comparer { get; set; }//>
        public DmlValueExpression Value { get; set; }//'b'
        public override DmlExpressionType NodeType => DmlExpressionType.Filter;
    }
}

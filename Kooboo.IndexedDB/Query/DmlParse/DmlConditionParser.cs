using ConditionParser.Expressions;
using ConditionParser.Models;
using ConditionParser.Modes;
using System.Linq;

namespace ConditionParser
{
    public static class DmlConditionParser
    {

        public static DmlExpression Parse(string condition)
        {
            var iterator = new Iterator(condition);
            iterator.TrimStart();
            return Analyze(iterator, null, null);
        }

        static DmlExpression Analyze(Iterator iterator, DmlExpression left, Operand? operand)
        {
            if (iterator.Current == '(')
            {
                iterator.Next();
                var result = Analyze(iterator, null, null);
                result = Merge(left, operand, result);
                if (iterator.Current != ')') throw new ConditionParseException(iterator.Position);
                iterator.Next();
                iterator.TrimStart();
                if (iterator.End || iterator.Current == ')') return result;
                if (!iterator.IsOperand()) throw new ConditionParseException(iterator.Position);
                var mOperand = iterator.ExtractOperand();
                result = Analyze(iterator, result, mOperand);
                return result;
            }
            else
            {
                var filter = GetFilter(iterator);
                var result = Merge(left, operand, filter);
                if (iterator.End || iterator.Current == ')') return result;
                if (!iterator.IsOperand()) throw new ConditionParseException(iterator.Position);
                var mOperand = iterator.ExtractOperand();
                result = Analyze(iterator, result, mOperand);
                return result;
            }
        }

        private static DmlFilterExpression GetFilter(Iterator iterator)
        {
            var filter = new DmlFilterExpression();
            if (!iterator.IsValue()) throw new ConditionParseException(iterator.Position);
            filter.Property = iterator.ExtractValue(true).Value.ToString();
            if (!iterator.IsComparer()) throw new ConditionParseException(iterator.Position);
            filter.Comparer = iterator.ExtractComparer();
            if (!iterator.IsValue()) throw new ConditionParseException(iterator.Position);
            filter.Value = iterator.ExtractValue();
            return filter;
        }

        static DmlExpression Merge(DmlExpression left, Operand? operand, DmlExpression right)
        {
            if (operand.HasValue)
            {
                return new DmlBinaryExpression
                {
                    Left = left,
                    Operand = operand.Value,
                    Right = right
                };
            }

            return left ?? right;
        }

    }
}

using Kooboo.Data.Context;
using Kooboo.Data.Definition;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule
{
    public abstract class ConditionDefineBase<T> : IConditionDefine where T : TargetModelBase<T>
    {
        private readonly Comparer[] _comparers;
        public abstract string Name { get; }
        public virtual Comparer[] Comparers => _comparers;
        public abstract ConditionValueType ValueType { get; }

        public ConditionDefineBase()
        {
            switch (ValueType)
            {
                case ConditionValueType.String:
                    _comparers = new[] { Comparer.Contains, Comparer.EqualTo, Comparer.NotEqualTo, Comparer.StartWith };
                    break;
                case ConditionValueType.Datetime:
                case ConditionValueType.Number:
                    _comparers = new[] { Comparer.GreaterThan, Comparer.GreaterThanOrEqual, Comparer.EqualTo, Comparer.NotEqualTo, Comparer.LessThan, Comparer.LessThanOrEqual };
                    break;
                case ConditionValueType.Boolean:
                default:
                    _comparers = new[] { Comparer.EqualTo, Comparer.NotEqualTo };
                    break;
            }
        }

        protected abstract object GetPropertyValue(T model);
        public virtual bool Match(T model, Comparer comparer, string value)
        {
            var propertyValue = GetPropertyValue(model);

            switch (ValueType)
            {
                case ConditionValueType.String:
                    return ComparerString(propertyValue, comparer, value);
                case ConditionValueType.Number:
                    return ComparerNumber(propertyValue, comparer, value);
                case ConditionValueType.Datetime:
                    return ComparerDatetime(propertyValue, comparer, value);
                case ConditionValueType.Boolean:
                    return ComparerBoolean(propertyValue, comparer, value);
                default:
                    return ComparerId(propertyValue, comparer, value);
            }

        }

        bool ComparerString(object left, Comparer comparer, string value)
        {
            var property = (left?.ToString() ?? "").Trim();

            switch (comparer)
            {
                case Comparer.EqualTo:
                    return property == value;
                case Comparer.GreaterThan:
                    return false;
                case Comparer.GreaterThanOrEqual:
                    return false;
                case Comparer.LessThan:
                    return false;
                case Comparer.LessThanOrEqual:
                    return false;
                case Comparer.NotEqualTo:
                    return property != value;
                case Comparer.StartWith:
                    return property.StartsWith(value);
                case Comparer.Contains:
                    return property.Contains(value);
                default:
                    return false;
            }
        }

        bool ComparerBoolean(object left, Comparer comparer, string value)
        {

            if (!bool.TryParse(value, out var right)) return false;

            try
            {
                var property = Convert.ToBoolean(left);

                switch (comparer)
                {
                    case Comparer.EqualTo:
                        return property == right;
                    case Comparer.GreaterThan:
                        return false;
                    case Comparer.GreaterThanOrEqual:
                        return false;
                    case Comparer.LessThan:
                        return false;
                    case Comparer.LessThanOrEqual:
                        return false;
                    case Comparer.NotEqualTo:
                        return property != right;
                    case Comparer.StartWith:
                        return false;
                    case Comparer.Contains:
                        return false;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        bool ComparerNumber(object left, Comparer comparer, string value)
        {

            if (!decimal.TryParse(value, out var right)) return false;

            try
            {
                var property = Convert.ToDecimal(left);

                switch (comparer)
                {
                    case Comparer.EqualTo:
                        return property == right;
                    case Comparer.GreaterThan:
                        return property > right;
                    case Comparer.GreaterThanOrEqual:
                        return property >= right;
                    case Comparer.LessThan:
                        return property < right;
                    case Comparer.LessThanOrEqual:
                        return property <= right;
                    case Comparer.NotEqualTo:
                        return property != right;
                    case Comparer.StartWith:
                        return false;
                    case Comparer.Contains:
                        return false;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        bool ComparerDatetime(object left, Comparer comparer, string value)
        {

            if (!DateTime.TryParse(value, out var right)) return false;

            try
            {
                var property = Convert.ToDateTime(left);

                switch (comparer)
                {
                    case Comparer.EqualTo:
                        return property == right;
                    case Comparer.GreaterThan:
                        return property > right;
                    case Comparer.GreaterThanOrEqual:
                        return property >= right;
                    case Comparer.LessThan:
                        return property < right;
                    case Comparer.LessThanOrEqual:
                        return property <= right;
                    case Comparer.NotEqualTo:
                        return property != right;
                    case Comparer.StartWith:
                        return false;
                    case Comparer.Contains:
                        return false;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        bool ComparerId(object left, Comparer comparer, string value)
        {
            var property = left?.ToString() ?? "";

            switch (comparer)
            {
                case Comparer.EqualTo:
                    return property.Equals(value);
                case Comparer.GreaterThan:
                    return false;
                case Comparer.GreaterThanOrEqual:
                    return false;
                case Comparer.LessThan:
                    return false;
                case Comparer.LessThanOrEqual:
                    return false;
                case Comparer.NotEqualTo:
                    return !property.Equals(value);
                case Comparer.StartWith:
                    return false;
                case Comparer.Contains:
                    return false;
                default:
                    return false;
            }
        }
    }
}

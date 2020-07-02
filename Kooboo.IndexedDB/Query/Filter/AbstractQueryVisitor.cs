using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kooboo.IndexedDB.Query
{
    internal class AbstractQueryVisitor<TKey,TValue>
    {
        internal ObjectStoreVisitor store;

        public AbstractQueryVisitor(ObjectStore store)
        {
            this.store = store;
        }

        public IQuery Skip(int skipNum)
        {
            return new QuerySkip(skipNum, store);
        }

        public IQuery EqualInExtend<T>(Expression<Func<TValue, object>> FieldExpression, IList<T> Values)
        {
            string field = GetFieldName(FieldExpression.Body);
            IQuery first = new QueryEquals(field, Values.First(), store);
            foreach (var item in Values.Skip(1))
            {
                first = new QueryOr(first, new QueryEquals(field, item, store));
            }
            return first;
        }

        public IQuery OrderPrimaryKey(bool ascending)
        {
            return new QueryOrderPrimaryKey(ascending, store);
        }

        public IQuery Order(Expression<Func<TValue, object>> expression, bool ascending)
        {
            return new QueryOrder(GetFieldName(expression.Body), ascending, store);
        }

        public IQuery Decompose(Expression<Predicate<TValue>> predicate)=> Visit(((LambdaExpression)predicate).Body);

        private IQuery Visit(Expression expr)
        {
            if (expr is MemberExpression mem && expr.Type == typeof(bool))
            {
                return new QueryEquals(GetFieldName(mem), true, store);
            }
            else if (expr.NodeType == ExpressionType.Not)
            {
                var unary = expr as UnaryExpression;
                return new QueryNot(Visit(unary.Operand), store);
            }
            else if (expr.NodeType == ExpressionType.Equal)
            {
                var bin = expr as BinaryExpression;
                return new QueryEquals(GetFieldName(bin.Left), this.GetValue(bin.Right), store);
            }
            else if (expr.NodeType == ExpressionType.NotEqual)
            {
                var bin = expr as BinaryExpression;
                return new QueryNotEquals(GetFieldName(bin.Left), this.GetValue(bin.Right), store);
            }
            else if (expr.NodeType == ExpressionType.LessThan)
            {
                var bin = expr as BinaryExpression;
                return new QueryLessThan(GetFieldName(bin.Left), GetValue(bin.Right), store);
            }
            else if (expr.NodeType == ExpressionType.LessThanOrEqual)
            {
                var bin = expr as BinaryExpression;
                return new QueryLessThanOrEqual(this.GetFieldName(bin.Left), this.GetValue(bin.Right), store);
            }
            else if (expr.NodeType == ExpressionType.GreaterThan)
            {
                var bin = expr as BinaryExpression;
                return new QueryGreaterThan(this.GetFieldName(bin.Left), this.GetValue(bin.Right), store);
            }
            else if (expr.NodeType == ExpressionType.GreaterThanOrEqual)
            {
                var bin = expr as BinaryExpression;
                return new QueryGreaterThanOrEqual(this.GetFieldName(bin.Left), this.GetValue(bin.Right), store);
            }
            else if (expr.NodeType == ExpressionType.AndAlso)
            {
                var bin = expr as BinaryExpression;
                var left = this.Visit(bin.Left);
                var right = this.Visit(bin.Right);
                return new QueryAnd(left, right);
            }
            else if (expr.NodeType == ExpressionType.OrElse)
            {
                var bin = expr as BinaryExpression;
                var left = this.Visit(bin.Left);
                var right = this.Visit(bin.Right);

                return new QueryOr(left, right);
            }
            else if (expr.NodeType == ExpressionType.Constant)
            {
                var constant = expr as ConstantExpression;
                if ((bool)constant.Value)
                    return new QueryAll(store);
                return new QueryEmpty();
            }
            else if (expr is MethodCallExpression)
            {
                var met = expr as MethodCallExpression;
                var method = met.Method.Name;
                var type = met.Method.DeclaringType;

                if (method == "StartsWith")
                {
                    return new QueryStartsWith(this.GetFieldName(met.Object), GetValue(met.Arguments[0]), store);
                }
                else if (method == "Equals")
                {
                    return new QueryEquals(this.GetFieldName(met.Object), GetValue(met.Arguments[0]), store);
                }
                else if (method == "Contains" && type == typeof(string))
                {
                    return new QueryContains(this.GetFieldName(met.Object), GetValue(met.Arguments[0]), store);
                }
            }

            throw new NotImplementedException("Not implemented Linq expression");
        }

        private object GetValue(Expression expr)
        {
            if (expr is ConstantExpression cons)
            {
                return cons.Value;
            }
            if (expr.NodeType == ExpressionType.Convert)
            {
                UnaryExpression unary = expr as UnaryExpression;
                MemberExpression member = unary.Operand as MemberExpression;
                return Expression.Lambda<Func<object>>(Expression.Convert(member, typeof(object))).Compile().Invoke();
            }

            try
            {
                //such as call/member.access...
                return Expression.Lambda<Func<object>>(Expression.Convert(expr, typeof(object))).Compile().Invoke();
            }
            catch
            {
                throw new NotImplementedException("Not implemented Linq expression");
            }
        }

        private string GetFieldName(Expression expr)
        {
            if (expr is MemberExpression mem)
            {
                return mem.Member.Name;
            }
            if (expr.NodeType == ExpressionType.Convert)
            {
                UnaryExpression unary = expr as UnaryExpression;
                MemberExpression member = unary.Operand as MemberExpression;
                return member.Member.Name;
            }

            throw new Exception("operation not supported yet, please report " + expr.NodeType.ToString());
        }
    }
}

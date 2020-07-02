using ConditionParser;
using ConditionParser.Expressions;
using Kooboo.IndexedDB.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.IndexedDB.Query
{
    public class WhereFilter2<TKey, TValue>
    {
        private ObjectStore<TKey, TValue> _store;

        private AbstractQueryVisitor<TKey, TValue> handler;
        private IQuery query;
        private int skip;

        //private ReadLock readLock => store.readLock;

        public WhereFilter2(ObjectStore<TKey, TValue> store)
        {
            _store = store;
            handler = new AbstractQueryVisitor<TKey, TValue>(store);
        }

        public WhereFilter2(ObjectStore<TKey, TValue> store, Expression<Predicate<TValue>> func) : this(store)
        {
            handler = new AbstractQueryVisitor<TKey, TValue>(store);
            //using (readLock.EnterSearchLock())
            query = handler.Decompose(func);
        }

        //Where(a==3).Where(b==4)
        //where( a==3 and b==4 )
        public WhereFilter2<TKey, TValue> Where(Expression<Predicate<TValue>> predicate)
        {
            //using (readLock.EnterSearchLock())
            {
                if (query == null)
                    query = handler.Decompose(predicate);
                else
                    query = new QueryAnd(query, handler.Decompose(predicate));
                return this;
            }
        }

        public WhereFilter2<TKey, TValue> OrderByAscending(Expression<Func<TValue, object>> expression)
        {
            //using (readLock.EnterSearchLock())
            {
                if (query == null)
                    query = handler.Order(expression, true);
                else
                    query = new QueryAnd(query, handler.Order(expression, true));
                return this;
            }
        }

        public WhereFilter2<TKey, TValue> OrderByDescending(Expression<Func<TValue, object>> expression)
        {
            //using (readLock.EnterSearchLock())
            {
                if (query == null)
                    query = handler.Order(expression, false);
                else
                    query = new QueryAnd(query, handler.Order(expression, false));
                return this;
            }
        }

        public WhereFilter2<TKey, TValue> OrderByAscending()
        {
            //using (readLock.EnterSearchLock())
            {
                if (query == null)
                    query = handler.OrderPrimaryKey(true);
                else
                    query = new QueryAnd(query, handler.OrderPrimaryKey(true));
                return this;
            }
        }

        public WhereFilter2<TKey, TValue> OrderByDescending()
        {
            //using (readLock.EnterSearchLock())
            {
                if (query == null)
                    query = handler.OrderPrimaryKey(false);
                else
                    query = new QueryAnd(query, handler.OrderPrimaryKey(false));
                return this;
            }
        }

        //e.name == Array[]
        public WhereFilter2<TKey, TValue> WhereIn<T>(Expression<Func<TValue, object>> FieldExpression, IList<T> Values)
        {
            //using (readLock.EnterSearchLock())
            {
                if (query == null)
                    query = handler.EqualInExtend(FieldExpression, Values);
                else
                    query = new QueryAnd(query, handler.EqualInExtend(FieldExpression, Values));
                return this;
            }
        }

        public WhereFilter2<TKey, TValue> Skip(int num)
        {
            skip = num;
            return this;
        }

        public int Count()
        {
            //using (readLock.EnterSearchLock())
            {
                return query.Execute(null).Count();
            }
        }

        public TValue FirstOrDefault()
        {
            foreach (var item in GetValues())
            {
                return item;
            }
            return default(TValue);
        }

        public IEnumerable<TValue> GetValues(int take = int.MaxValue, bool columnDataOnly = false)
        {
            return Execute(columnDataOnly).Skip(skip).Take(take);
        }

        public IList<TValue> GetList(int take = int.MaxValue, bool columnDataOnly = false)
        {
            return GetValues(take, columnDataOnly).ToList();
        }

        private IEnumerable<TValue> Execute(bool columnDataOnly = false)
        {
            var local = query;
            if (local == null)
                local = new QueryAll((ObjectStoreVisitor)_store);
            if (columnDataOnly)
            {
                foreach (var item in local.Execute(null))
                {
                    yield return this._store.InternalGetValueFromColumns(item);
                }
            }
            else
            {
                foreach (var item in local.Execute(null))
                {
                    yield return this._store.InternalGetValue(item);
                }
            }
        }
    }
}

using Kooboo.IndexedDB.Btree;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.IndexedDB.Query
{
    internal class QueryLessThan : FieldRelationalQuery
    {
        private object _value;

        public QueryLessThan(string field, object value,ITableVisitor store)
            : base(field, store)
        {
            _value = value;
        }

        public override IEnumerable<long> Execute(IEnumerable<long> collection)
        {
            if (base.hasColumn)
            {
                if (collection == null)
                    collection = DefaultColloction;
                var evaluator = ColumnEvaluator.GetEvaluator(base.columnType, base.columnLength, base.columnToBytes, base.columnRelativePosition, Comparer.LessThan, _value);
                return evaluator.Execute(collection, store);
            }
            else
            {
                ItemCollection result = new IndexLessQuery(_value, index).Query();
                if (collection == null)
                    return result;
                return collection.Intersect(result);
            }
        }
    }
}

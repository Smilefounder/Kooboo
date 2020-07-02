using Kooboo.IndexedDB.Btree;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.IndexedDB.Query
{
    // a < 3 && b > 5
    // left 交集 right -> 结果集A
    // where (e=> e.id > 3 && e.price > 5 ).order( e => e.date ).skip(10) Index,column
    //foreach()
    //    match(e.id)
    //      add
    internal class QueryGreaterThanOrEqual : FieldRelationalQuery
    {
        private object _value;

        public QueryGreaterThanOrEqual(string field, object value, ITableVisitor store)
            : base(field, store)
        {
            _value = RealValue(value);
        }

        public override IEnumerable<long> Execute(IEnumerable<long> collection)
        {

            if (base.isColumn)
            {
                if (collection == null)
                    collection = DefaultColloction;
                var evaluator = ColumnEvaluator.GetEvaluator(base.columnType, base.columnLength, base.columnToBytes, base.columnRelativePosition, Comparer.GreaterThanOrEqual, _value);
                return evaluator.Execute(collection, store);
            }
            else
            {
                ItemCollection result = new IndexGreaterThanOrEqualQuery(_value, index).Query();
                if (collection == null)
                    return result;
                return collection.Intersect(result);
            }
        }
    }
}

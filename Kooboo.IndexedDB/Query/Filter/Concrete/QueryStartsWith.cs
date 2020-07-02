using Kooboo.IndexedDB.Btree;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.IndexedDB.Query
{
    internal class QueryStartsWith: FieldRelationalQuery
    {
        private object _value;

        public QueryStartsWith(string field, object value, ITableVisitor store)
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
                var evaluator = ColumnEvaluator.GetEvaluator(base.columnType, base.columnLength, base.columnToBytes, base.columnRelativePosition, Comparer.StartWith, _value);
                return evaluator.Execute(collection, store);
            }
            else
            {
                ItemCollection result = new IndexStartWithQuery(_value, index).Query();
                if (collection == null)
                    return result;
                return collection.Intersect(result);

            }
        }
    }
}

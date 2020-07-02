using Kooboo.IndexedDB.Btree;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.IndexedDB.Query
{
    //a==3(decimal)
    internal class QueryEquals : FieldRelationalQuery
    {
        private object _value;

        public QueryEquals(string field, object value, ITableVisitor store)
            : base(field, store)
        {
            _value = RealValue(value);
        }

        public override IEnumerable<long> Execute(IEnumerable<long> collection)
        {
            if (base.isColumn)//对column的 equal操作
            {
                if (collection == null)
                    collection = DefaultColloction;
                var evaluator = ColumnEvaluator.GetEvaluator(base.columnType, base.columnLength, base.columnToBytes, base.columnRelativePosition, Comparer.EqualTo, _value);
                return evaluator.Execute(collection, store);
            }
            else//index  对index的equal操作
            {
                ItemCollection result = new IndexEqualQuery(_value, index).Query();
                if (collection == null)
                    return result;
                return collection.Intersect(result);
            }
        }
    }
}

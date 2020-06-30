using System.Collections.Generic;

namespace Kooboo.IndexedDB.Query
{
    internal class QueryAll : IQuery
    {
        internal ITableVisitor _store;

        public QueryAll(ITableVisitor store)
        {
            this._store = store;
        }

        public IEnumerable<long> Execute(IEnumerable<long> collection)
        {
            if (collection == null)
                return _store.GetCollection(true);
            return collection;
        }
    }
}

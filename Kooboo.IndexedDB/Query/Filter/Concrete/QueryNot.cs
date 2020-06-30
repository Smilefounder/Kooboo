using System.Collections.Generic;
using System.Linq;

namespace Kooboo.IndexedDB.Query
{
    public class QueryNot : IQuery
    {
        private IQuery _one;
        private ITableVisitor _store;

        internal QueryNot(IQuery one, ITableVisitor store)
        {
            _one = one;
            _store = store;
        }

        public IEnumerable<long> Execute(IEnumerable<long> collection)
        {
            if (collection == null)
                collection = _store.GetCollection(true);
            return collection.Except(_one.Execute(null));
        }
    }
}

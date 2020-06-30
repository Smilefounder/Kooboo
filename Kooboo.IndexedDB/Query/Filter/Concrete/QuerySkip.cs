using System.Collections.Generic;
using System.Linq;

namespace Kooboo.IndexedDB.Query
{
    internal class QuerySkip : IQuery
    {
        private int skipNum;
        private ITableVisitor store;
        public QuerySkip(int skipNum, ITableVisitor store)
        {
            this.skipNum = skipNum;
            this.store = store;
        }

        public IEnumerable<long> Execute(IEnumerable<long> collection)
        {
            if (collection == null)
                return store.GetCollection(true).Skip(skipNum);
            return collection.Skip(skipNum);
        }
    }
}

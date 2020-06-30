using System.Collections.Generic;
using System.Linq;

namespace Kooboo.IndexedDB.Query
{
    public class QueryEmpty : IQuery
    {
        public IEnumerable<long> Execute(IEnumerable<long> collection)
        {
            return Enumerable.Empty<long>();
        }
    }
}

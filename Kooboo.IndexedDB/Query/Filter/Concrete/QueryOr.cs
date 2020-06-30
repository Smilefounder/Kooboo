using System.Collections.Generic;
using System.Linq;

namespace Kooboo.IndexedDB.Query
{
    public class QueryOr : IQuery
    {
        private IQuery _left;
        private IQuery _right;

        internal QueryOr(IQuery left, IQuery right)
        {
            _left = left;
            _right = right;
        }

        public IEnumerable<long> Execute(IEnumerable<long> collection)
        {
            return _left.Execute(collection).Union(_right.Execute(collection));
        }
    }
}

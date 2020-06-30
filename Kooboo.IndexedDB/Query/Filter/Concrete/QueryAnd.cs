using System.Collections.Generic;

namespace Kooboo.IndexedDB.Query
{
    public class QueryAnd : IQuery
    {
        private IQuery _left;
        private IQuery _right;

        internal QueryAnd(IQuery left, IQuery right)
        {
            _left = left;
            _right = right;
        }

        public IEnumerable<long> Execute(IEnumerable<long> collection)
        {
            return _right.Execute(_left.Execute(collection));
        }
    }
}

using System.Collections.Generic;

namespace Kooboo.IndexedDB.Query
{
    internal interface IQuery
    {
        IEnumerable<long> Execute(IEnumerable<long> collection);
    }
}

using Kooboo.IndexedDB.Btree;
using Kooboo.IndexedDB.Indexs;

namespace Kooboo.IndexedDB.Query
{ 
    public abstract class IndexRangeQuery
    {
        private IIndex _Index;

        /// <summary>
        /// This value is the lower-bound of the key range.
        /// </summary>
        public abstract byte[] lower { get; }

        /// <summary>
        /// This value is the upper-bound of the key range.
        /// </summary>
        public abstract byte[] upper { get; }

        /// <summary>
        /// Returns false if the lower-bound value is included in the key range. Returns true if the lower-bound value is excluded from the key range.
        /// </summary>
        public abstract bool lowerOpen { get; }

        /// <summary>
        /// Returns false if the upper-bound value is included in the key range. Returns true if the upper-bound value is excluded from the key range.
        /// </summary>
        public abstract bool upperOpen { get; }

        public IndexRangeQuery(IIndex index)
        {
            _Index = index;
        }

        public ItemCollection Query()
        {
            return _Index.GetCollection(lower, upper, lowerOpen, upperOpen, true);
        }
    }
}

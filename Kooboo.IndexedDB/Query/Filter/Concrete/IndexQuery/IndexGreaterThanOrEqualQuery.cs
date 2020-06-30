using Kooboo.IndexedDB.ByteConverter;
using Kooboo.IndexedDB.Indexs;

namespace Kooboo.IndexedDB.Query
{

    public class IndexGreaterThanOrEqualQuery: IndexRangeQuery
    {
        private byte[] value;

        public IndexGreaterThanOrEqualQuery(object value, IIndex index) : base(index)
        {
            this.value = index.GetBytes(value); 
        }
        public override byte[] lower => value;
        public override byte[] upper => null;
        public override bool lowerOpen => false;
        public override bool upperOpen => false;
    }
}

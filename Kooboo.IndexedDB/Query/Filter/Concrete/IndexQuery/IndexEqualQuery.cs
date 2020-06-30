using Kooboo.IndexedDB.ByteConverter;
using Kooboo.IndexedDB.Indexs;

namespace Kooboo.IndexedDB.Query
{

    public class IndexEqualQuery : IndexRangeQuery
    {
        private byte[] value;

        public IndexEqualQuery(object value, IIndex index) : base(index)
        {
            this.value = index.GetBytes(value); ;
        }

        public override byte[] lower => value;
        public override byte[] upper => value;
        public override bool lowerOpen => false;
        public override bool upperOpen => false;
    }
}

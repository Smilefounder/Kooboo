using Kooboo.IndexedDB.ByteConverter;
using Kooboo.IndexedDB.Indexs;

namespace Kooboo.IndexedDB.Query
{

    public class IndexLessQuery : IndexRangeQuery
    {
        private byte[] value;

        public IndexLessQuery(object value, IIndex index) : base(index)
        {
            this.value = index.GetBytes(value);
        }
        public override byte[] lower => null;
        public override byte[] upper => value;
        public override bool lowerOpen => false;
        public override bool upperOpen => true;
    }
}

using Kooboo.IndexedDB.ByteConverter;
using Kooboo.IndexedDB.Indexs;

namespace Kooboo.IndexedDB.Query
{
   
    public class IndexGreaterThanQuery : IndexRangeQuery
    {
        private byte[] value;

        public IndexGreaterThanQuery(object value, IIndex index) : base(index)
        {
            this.value = index.GetBytes(value);
        }
        public override byte[] lower => value;
        public override byte[] upper => null;
        public override bool lowerOpen => true;
        public override bool upperOpen => false;
    }
}

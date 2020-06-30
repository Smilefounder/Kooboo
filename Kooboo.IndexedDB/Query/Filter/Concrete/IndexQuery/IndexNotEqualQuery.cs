using Kooboo.IndexedDB.ByteConverter;
using Kooboo.IndexedDB.Indexs;
using System;

namespace Kooboo.IndexedDB.Query
{

    public class IndexNotEqualQuery : IndexRangeQuery
    {
        private byte[] value;

        public IndexNotEqualQuery(object value, IIndex index) : base(index)
        {
            this.value = index.GetBytes(value);
        }

        public override byte[] lower => throw new NotImplementedException();

        public override byte[] upper => throw new NotImplementedException();

        public override bool lowerOpen => throw new NotImplementedException();

        public override bool upperOpen => throw new NotImplementedException();
    }
}

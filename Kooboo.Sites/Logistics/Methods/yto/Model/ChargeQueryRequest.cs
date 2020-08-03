using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.yto.Model
{
    public class ChargeQueryRequest
    {
        public string StartProvince { get; set; }

        public string StartCity { get; set; }

        public string EndProvince { get; set; }

        public string EndCity { get; set; }

        public string GoodsWeight { get; set; }

        public string GoodsLength { get; set; }

        public string GoodsWidth { get; set; }

        public string GoodsHeight { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.deppon.Model
{
    public class PostageResponse
    {
        public string Reason { get; set; }

        public ResponseParam ResponseParam { get; set; }
    }

    public class ResponseParam
    {
        public string LogisticCompanyID { get; set; }

        public List<PriceInfo> PriceInfo { get; set; }
    }


    public class PriceInfo
    {
        public string LowestPrice { get; set; }
    }
}

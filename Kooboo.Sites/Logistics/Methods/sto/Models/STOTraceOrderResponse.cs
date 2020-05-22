using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.sto.Models
{
    public class STOTraceOrderResponse
    {
        public bool success { get; set; }
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
        public string needRetry { get; set; }
        public string requestId { get; set; }
        public string expInfo { get; set; }
        public ResponseData data { get; set; }
    }

    public class ResponseData
    {
        public List<waybillNoInfo> waybillNo { get; set; }
    }

    public class waybillNoInfo
    {
        public string waybillNo { get; set; }
        public string opOrgName { get; set; }
        public string opOrgCode { get; set; }
        public string opOrgCityName { get; set; }
        public string opOrgProvinceName { get; set; }
        public string opOrgTel { get; set; }
    }
}

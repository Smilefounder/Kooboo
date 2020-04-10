using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.zop.Models
{
    public class TraceOrderResponse
    {
        public string BillCode { get; set; }

        public List<TraceInfo> Traces { get; set; }
    }

    public class TraceInfo
    {
        public string Desc { get; set; }

        public string IsCenter { get; set; }

        public string ScanCity { get; set; }

        public string ScanDate { get; set; }

        public string ScanProv { get; set; }

        public string ScanSite { get; set; }

        public string ScanSiteCode { get; set; }

        public string ScanType { get; set; }
    }
}

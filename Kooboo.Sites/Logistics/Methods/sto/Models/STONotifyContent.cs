using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.sto.Models
{
    public class STONotifyContent
    {
        public string linkCode { get; set; }
        public string waybillNo { get; set; }
        public TraceInfo trace { get; set; }
    }

    public class TraceInfo
    {
        public string memo { get; set; }
        public string opOrgProvinceName { get; set; }
        public string opOrgCityName { get; set; }
        public string signoffPeople { get; set; }
        public string opOrgName { get; set; }
        public string scanType { get; set; }
    }
}

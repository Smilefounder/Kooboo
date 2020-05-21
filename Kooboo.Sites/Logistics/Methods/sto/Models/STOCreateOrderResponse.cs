using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.sto.Models
{
    public class STOCreateOrderResponse
    {
        public bool success { get; set; }
        public Data data { get; set; }
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
    }

    public class Data
    {
        public string orderNo { get; set; }
        public string packagePlace { get; set; }
        public string bigWord { get; set; }
        public string waybillNo { get; set; }
    }
}

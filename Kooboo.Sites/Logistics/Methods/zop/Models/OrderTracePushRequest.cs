using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.zop.Models
{
    public class OrderTracePushRequest
    {
        public string BillCode { get; set; }

        public string Desc { get; set; }

        public string ActionTime { get; set; }
    }
}

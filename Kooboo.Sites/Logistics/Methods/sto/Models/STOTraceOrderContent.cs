using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.sto.Models
{
    public class STOTraceOrderContent
    {
        public string order { get; set; }

        public List<string> waybillNoList { get; set; }
    }
}

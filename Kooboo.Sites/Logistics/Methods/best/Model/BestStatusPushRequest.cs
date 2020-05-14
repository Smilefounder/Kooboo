using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.best.Model
{
    public class BestStatusPushRequest
    {
        public string txLogisticID { get; set; }
        public string mailNo { get; set; }
        public string remark { get; set; }
        public string status { get; set; }
    }
}

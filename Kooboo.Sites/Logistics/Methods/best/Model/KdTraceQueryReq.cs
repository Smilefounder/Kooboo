using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.best.Model
{
    public class KdTraceQueryReq
    {
        public MailNos mailNos { get; set; }
    }

    public class MailNos
    {
        public List<String> mailNo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.best.Model
{
    public class BestTraceOrderRequest
    {
        public MailNos mailNos { get; set; }
    }

    public class MailNos
    {
        public List<string> mailNo { get; set; }
    }
}

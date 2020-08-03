using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.sto
{
    public class STOSetting : ILogisticsSetting
    {
        public string Name => "STO";

        public bool IsProductEnvironment { get; set; }

        public string SecretKey { get; set; }

        public string FromAppkey { get; set; }

        public string FromCode { get; set; }

        public string CustomerSiteCode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerSitePassword { get; set; }

        public string MonthCustomerCode { get; set; }

        public string ServerURL => IsProductEnvironment ? "https://cloudinter-linkgatewayonline.sto.cn/gateway/link.do" : "http://cloudinter-linkgatewaytest.sto.cn/gateway/link.do";
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.zop
{
    public class ZOPSetting : ILogisticsSetting
    {
        private const string name = "ZTO";

        public bool UseSandBox { get; set; }

        public string Name => name;

        public string CompanyId { get; set; }

        public string Key { get; set; }

        public string CompanyCode { get; set; }

        public string HallCode { get; set; }

        public string ServiceCode { get; set; }

        public string Actions { get; set; }

        public string ProblemCodes { get; set; }

        public string ServerURL => UseSandBox ? "http://58.40.16.122:8080" : "http://japi.zto.cn";
    }
}

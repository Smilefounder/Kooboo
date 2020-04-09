using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.zto
{
    public class ZTOSetting : ILogisticsSetting
    {
        private const string name = "ZTO";

        public bool UseSandBox { get; set; }

        public string Name => name;

        public string CompanyId { get; set; }

        public string Key { get; set; }

        public string BackUrl { get; set; }

        public string CompanyCode { get; set; }

        public string HallCode { get; set; }

        public string ServiceCode { get; set; }

        public string ServerURL => UseSandBox ? "http://58.40.16.122:8080" : "http://japi.zto.cn";
    }
}

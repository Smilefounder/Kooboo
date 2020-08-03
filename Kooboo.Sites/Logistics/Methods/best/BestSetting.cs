using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.best
{
    public class BestSetting : ILogisticsSetting
    {
        private const string name = "Best";

        public bool UseSandBox { get; set; }

        public string Name => name;

        public string PartnerID { get; set; }

        public string PartnerKey { get; set; }

        public string ServerURL => UseSandBox ? "http://kdtest.800best.com/kd/api/process" : "";
    }
}

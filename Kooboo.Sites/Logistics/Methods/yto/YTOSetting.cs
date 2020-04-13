using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.yto
{
    public class YTOSetting : ILogisticsSetting
    {
        private const string name = "YTO";

        public bool UseSandBox { get; set; }

        public string Name => name;

        public string AppKey { get; set; }

        public string Version { get; set; }

        public string UserId { get; set; }

        public string SecretKey { get; set; }

        public string ServerURL => UseSandBox ? "http://opentestapi.yto.net.cn/service" : "http://japi.zto.cn";
    }
}

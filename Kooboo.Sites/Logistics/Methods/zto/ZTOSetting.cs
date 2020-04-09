using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.uda
{
    public class ZTOSetting : ILogisticsSetting
    {
        private const string name = "ZTO";

        public string Name => name;

        public string AppKey { get; set; }

        public string AppSecret { get; set; }

        public string backurl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.sf
{
    public class SFSetting : ILogisticsSetting
    {
        private const string name = "SF";

        public bool UseSandBox { get; set; }

        public string Name => name;

        public string CheckWord { get; set; }

        public string ClientCode { get; set; }

        public string Lang { get; set; }

        public string CustId { get; set; }

        public string ServerURL => UseSandBox ? "http://bsp-oisp.sf-express.com/bsp-oisp/sfexpressService" : "";
    }
}

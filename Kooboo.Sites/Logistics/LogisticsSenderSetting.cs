using Kooboo.Data.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics
{
    public class LogisticsSenderSetting : ISiteSetting
    {
        public string Name => "LogisticsSender";

        public string SenderName { get; set; }

        public string SenderPhone { get; set; }

        public string SenderProvince { get; set; }

        public string SenderCity { get; set; }

        public string SenderCounty { get; set; }

        public string SenderAddress { get; set; }
    }
}

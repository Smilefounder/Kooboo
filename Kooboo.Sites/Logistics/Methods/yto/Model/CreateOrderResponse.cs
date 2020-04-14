using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kooboo.Sites.Logistics.Methods.yto.Model
{
    public class CreateOrderResponse
    {
        [XmlElement("logisticProviderID")]
        public string LogisticProviderID { get; set; }

        [XmlElement("txLogisticID")]
        public string TxLogisticID { get; set; }

        [XmlElement("success")]
        public string Success { get; set; }

        [XmlElement("reason")]
        public string Reason { get; set; }
    }
}

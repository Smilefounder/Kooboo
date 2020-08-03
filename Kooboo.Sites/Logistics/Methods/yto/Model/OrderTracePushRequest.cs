using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kooboo.Sites.Logistics.Methods.yto.Model
{
    [XmlRoot("UpdateInfo")]
    public class OrderTracePushRequest
    {
        [XmlElement("logisticProviderID")]
        public string LogisticProviderID { get; set; }

        [XmlElement("txLogisticID")]
        public string TxLogisticID { get; set; }

        [XmlElement("infoContent")]
        public string InfoContent { get; set; }

        [XmlElement("remark")]
        public string Remark { get; set; }

        [XmlElement("questionCause")]
        public string QuestionCause { get; set; }
    }
}
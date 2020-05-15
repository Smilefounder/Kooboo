using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kooboo.Sites.Logistics.Methods.sf.Model
{
    [XmlRoot("Request")]
    public class OrderTracePushRequest
    {
        [XmlElement("orderNo")]
        public string OrderId { get; set; }
        [XmlElement("orderStateCode")]
        public string StateCode { get; set; }
        
        [XmlElement("orderStateDesc")]
        public string Description { get; set; }
        [XmlElement("lastTime")]
        public string Time { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kooboo.Sites.Logistics.Methods.sf.Model
{
    [XmlRoot("Response")]
    public class CreateOrderResponse
    {
        public string Head { get; set; }

        public OrderResponse Body { get; set; }
    }

    public class OrderResponse
    {
        [XmlAttribute("filter_result")]
        public string FilterResult { get; set; }

        [XmlAttribute("mailno")]
        public string MailNo { get; set; }
    }
}

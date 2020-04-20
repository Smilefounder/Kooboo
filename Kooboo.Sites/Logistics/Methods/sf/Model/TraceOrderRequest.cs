using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kooboo.Sites.Logistics.Methods.sf.Model
{
    [XmlRoot("RouteRequest")]
    public class TraceOrderRequest
    {
        [XmlAttribute("tracking_type")]
        public string TrackingType { get; set; }

        [XmlAttribute("method_type")]
        public string MethodType { get; set; }

        [XmlAttribute("tracking_number")]
        public string TrackingNumber { get; set; }
    }
}

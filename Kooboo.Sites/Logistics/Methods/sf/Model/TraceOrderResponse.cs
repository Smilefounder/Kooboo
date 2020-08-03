using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kooboo.Sites.Logistics.Methods.sf.Model
{
    [XmlRoot("Response")]
    public class TraceOrderResponse
    {
        public string Head { get; set; }

        public RouteResponse Body { get; set; }
    }

    public class RouteResponse
    {
        public List<Route> Route { get; set; }
    }

    public class Route
    {
        [XmlAttribute("accept_time")]
        public string AcceptTime { get; set; }

        [XmlAttribute("accept_address")]
        public string Address { get; set; }

        [XmlAttribute("remark")]
        public string Remark { get; set; }

        [XmlAttribute("opcode")]
        public string Code { get; set; }
    }
}

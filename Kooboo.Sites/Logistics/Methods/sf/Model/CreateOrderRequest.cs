using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kooboo.Sites.Logistics.Methods.sf.Model
{
    [XmlRoot("Body")]
    public class CreateOrderRequest
    {
        [XmlElement("Order")]
        public OrderInfo OrderInfo { get; set; }
    }

    public class OrderInfo
    {
        [XmlAttribute("orderid")]
        public string OrderID { get; set; }
        [XmlAttribute("j_company")]
        public string JCompany { get; set; }
        [XmlAttribute("j_contact")]
        public string JContact { get; set; }
        [XmlAttribute("j_tel")]
        public string JTel { get; set; }
        [XmlAttribute("mailno")]
        public string MailNo { get; set; }
        [XmlAttribute("j_shippercode")]
        public string JShippercode { get; set; }
        [XmlAttribute("j_address")]
        public string JAddress { get; set; }
        [XmlAttribute("j_country")]
        public string JCountry { get; set; }
        [XmlAttribute("j_province")]
        public string JProvince { get; set; }
        [XmlAttribute("j_city")]
        public string JCity { get; set; }
        [XmlAttribute("j_county")]
        public string JCounty { get; set; }
        [XmlAttribute("j_post_code")]
        public string JPostCode { get; set; }
        [XmlAttribute("d_company")]
        public string DCompany { get; set; }
        [XmlAttribute("d_contact")]
        public string DContact { get; set; }
        [XmlAttribute("d_tel")]
        public string DTel { get; set; }
        [XmlAttribute("d_deliverycode")]
        public string DDeliverycode { get; set; }
        [XmlAttribute("d_country")]
        public string DCountry { get; set; }
        [XmlAttribute("d_province")]
        public string DProvince { get; set; }
        [XmlAttribute("d_city")]
        public string DCity { get; set; }
        [XmlAttribute("d_address")]
        public string DAddress { get; set; }
        [XmlAttribute("d_county")]
        public string DCounty { get; set; }
        [XmlAttribute("d_post_code")]
        public string DPostCode { get; set; }
        [XmlAttribute("custid")]
        public string CustId { get; set; }
        [XmlAttribute("declared_value")]
        public string DeclaredValue { get; set; }
        [XmlElement("Cargo")]
        public Cargo Cargo { get; set; }
    }

    public class Cargo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("count")]
        public string Count { get; set; }
        [XmlAttribute("unit")]
        public string Unit { get; set; }
        [XmlAttribute("weight")]
        public string Weight { get; set; }
        [XmlAttribute("amount")]
        public string Amount { get; set; }
        [XmlAttribute("currency")]
        public string Currency { get; set; }
        [XmlAttribute("source_area")]
        public string SourceArea { get; set; }
    }

}

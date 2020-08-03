using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Kooboo.Sites.Logistics.Methods.yto.Model
{
    public class RequestOrder
    {
        [XmlElement("clientID")]
        public string ClientID { get; set; }
        [XmlElement("insuranceValue")]
        public string InsuranceValue { get; set; }//保价金额
        [XmlElement("txLogisticID")]
        public string TxLogisticID { get; set; }
        [XmlElement("orderType")]
        public string OrderType { get; set; }
        [XmlElement("serviceType")]
        public string ServiceType { get; set; }
        [XmlElement("special")]
        public string Special { get; set; }
        [XmlElement("logisticProviderID")]
        public string LogisticProviderID { get; set; }
        [XmlElement("receiver")]
        public PersonalInfo Receiver { get; set; }
        [XmlElement("sender")]
        public PersonalInfo Sender { get; set; }
        [XmlElement("items")]
        public List<Item> Items { get; set; }
    }

    public class PersonalInfo
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("postCode")]
        public string PostCode { get; set; }
        [XmlElement("phone")]
        public string Phone { get; set; }
        [XmlElement("prov")]
        public string Prov { get; set; }
        [XmlElement("city")]
        public string City { get; set; }
        [XmlElement("address")]
        public string Address { get; set; }
    }

    public class Item
    {
        public string ItemName { get; set; }

        public int Number { get; set; }
    }

}

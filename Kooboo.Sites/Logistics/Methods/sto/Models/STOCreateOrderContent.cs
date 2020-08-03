using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.sto.Models
{
    public class STOCreateOrderContent
    {
        public string orderNo { get; set; }
        public string orderSource { get; set; }
        public string billType { get; set; }
        public string orderType { get; set; }
        public sender sender { get; set; }
        public receiver receiver { get; set; }
        public cargo cargo { get; set; }
        public customer customer { get; set; }
        public internationalAnnex internationalAnnex { get; set; }
        public string waybillNo { get; set; }
        public assignAnnex assignAnnex { get; set; }
        public string codValue { get; set; }
        public string productType { get; set; }
        public List<string> serviceTypeList { get; set; }
        public string remark { get; set; }
        public string regionType { get; set; }
        public insuredAnnex insuredAnnex { get; set; }
    }

    public class sender
    {
        public string name { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public string postCode { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string area { get; set; }
        public string town { get; set; }
        public string address { get; set; }
    }

    public class receiver
    {
        public string name { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public string postCode { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string area { get; set; }
        public string town { get; set; }
        public string address { get; set; }
    }

    public class cargo
    {
        public string battery { get; set; }
        public string goodsType { get; set; }
        public string goodsName { get; set; }
        public int goodsCount { get; set; }
        public int spaceX { get; set; }
        public int spaceY { get; set; }
        public int spaceZ { get; set; }
        public int weight { get; set; }
        public string goodsAmount { get; set; }
        public List<cargoItemList> cargoItemList { get; set; }
    }

    public class cargoItemList
    {
        public string serialNumber { get; set; }
        public string referenceNumber { get; set; }
        public string productId { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public int unitPrice { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public int weight { get; set; }
        public string remark { get; set; }
    }

    // 正式环境找网点或市场部申请
    public class customer
    {
        public string siteCode { get; set; }
        public string customerName { get; set; }
        public string sitePwd { get; set; }
        public string monthCustomerCode { get; set; }
    }

    public class internationalAnnex
    {
        public string internationalProductType { get; set; }
        public bool customsDeclaration { get; set; }
        public string senderCountry { get; set; }
        public string receiverCountry { get; set; }
    }

    public class assignAnnex
    {
        public string takeCompanyCode { get; set; }
    }

    public class insuredAnnex
    {
        public string insuredValue { get; set; }
        public string goodsValue { get; set; }
    }
}

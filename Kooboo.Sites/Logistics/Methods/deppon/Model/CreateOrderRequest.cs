using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.deppon.Model
{
    public class CreateOrderRequest
    {
        public string LogisticID { get; set; }

        public string CompanyCode { get; set; }

        public string OrderType { get; set; }

        public string TransportType { get; set; }

        public string CustomerCode { get; set; }

        public Sender Sender { get; set; }


        public Receiver Receiver { get; set; }


        public PackageInfo PackageInfo { get; set; }

        public string GmtCommit { get; set; }

        public string PayType { get; set; }
    }

    public class Sender
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string Address { get; set; }
    }

    public class PackageInfo
    {
        public string CargoName { get; set; }

        public string TotalNumber { get; set; }

        public string TotalWeight { get; set; }

        public string DeliveryType { get; set; }
        
    }

    public class Receiver
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string Address { get; set; }
    }
}

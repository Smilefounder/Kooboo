using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.deppon.Model
{
    public class PostageRequest
    {
        public string LogisticCompanyID { get; set; }
        public string OriginalsStreet { get; set; }
        public string Originalsaddress { get; set; }
        public string SendDateTime { get; set; }
        public string TotalVolume { get; set; }
        public string TotalWeight { get; set; }
    }
}

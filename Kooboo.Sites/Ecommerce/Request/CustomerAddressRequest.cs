using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.Request
{
    public class CustomerAddressRequest
    {
        public string CountryName { get; set; }

        public string ProvinceName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string PostCode { get; set; }

        public string ConsigneeName { get; set; }

        public string ContactNumber { get; set; }
    }
}

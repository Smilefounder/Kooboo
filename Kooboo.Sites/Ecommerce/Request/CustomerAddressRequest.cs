using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.Request
{
    public class CustomerAddressRequest
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string PostCode { get; set; }

        public string Consignee { get; set; }

        public string ContactNumber { get; set; }

        public string HouseNumber { get; set; }

        public bool IsDefault { get; set; }
    }
}

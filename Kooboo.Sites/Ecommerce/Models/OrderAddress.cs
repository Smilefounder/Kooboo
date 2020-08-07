using Kooboo.Sites.Models;
using System;

namespace Kooboo.Sites.Ecommerce.Models
{
    public class OrderAddress
    {
        public OrderAddress()
        {

        }
        public OrderAddress(CustomerAddress address)
        {
            Address = address.Address;
            PostCode = address.PostCode;
            Address2 = address.Address2;
            HouseNumber = address.HouseNumber;
            City = address.City;
            Country = address.Country;
            Consignee = address.Consignee;
            ContactNumber = address.ContactNumber;
        }

        public string Address { get; set; }

        public string PostCode { get; set; }

        public string Address2 { get; set; }

        public string HouseNumber { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Consignee { get; set; }

        public string ContactNumber { get; set; }
    }
}

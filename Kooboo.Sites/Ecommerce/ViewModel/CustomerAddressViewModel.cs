using Kooboo.Sites.Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.ViewModel
{
    public class CustomerAddressViewModel
    {
        public CustomerAddressViewModel(CustomerAddress customerAddress)
        {
            this.Id = customerAddress.Id;
            this.CustomerId = customerAddress.CustomerId;
            this.Address = customerAddress.Address;
            this.PostCode = customerAddress.PostCode;
            this.Address2 = customerAddress.Address2;
            this.Country = customerAddress.Country;
            this.Consignee = customerAddress.Consignee;
            this.ContactNumber = customerAddress.ContactNumber;
        }

        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string Address { get; set; }

        public string PostCode { get; set; }

        public string Address2 { get; set; }

        public string Country { get; set; }

        public string Consignee { get; set; }

        public string ContactNumber { get; set; }
    }
}

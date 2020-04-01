using Kooboo.Sites.Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            this.DetailAddress = GetDetailAddress(customerAddress.Address);
        }

        public string GetDetailAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return string.Empty;
            }

            List<string> striparr = address.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            striparr = striparr.Where(s => !string.IsNullOrEmpty(s)).ToList();
            if (striparr.Count == 2)
            {
                return striparr[1];
            }

            return string.Empty;
        }

        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string Address { get; set; }

        public string DetailAddress { get; set; }

        public string PostCode { get; set; }

        public string Address2 { get; set; }

        public string Country { get; set; }

        public string Consignee { get; set; }

        public string ContactNumber { get; set; }
    }
}

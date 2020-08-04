using Kooboo.Sites.Models;
using System;

namespace Kooboo.Sites.Ecommerce.Models
{
    public class OrderAddress : CustomerAddress
    {
        public OrderAddress()
        {

        }
        public OrderAddress(CustomerAddress address, Guid orderId)
        {
            CustomerId = address.CustomerId;
            Address = address.Address;
            PostCode = address.PostCode;
            Address2 = address.Address2;
            HouseNumber = address.HouseNumber;
            City = address.City;
            Country = address.Country;
            Consignee = address.Consignee;
            ContactNumber = address.ContactNumber;
            OrderId = orderId;
        }
        /// <summary>
        /// For redundancy
        /// </summary>
        public Guid OrderId { get; set; }
    }
}

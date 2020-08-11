using Kooboo.Data.Context;
using Kooboo.Sites.Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.ViewModel
{
    public class CustomerViewModel
    {
        RenderContext context { get; set; }

        Customer customer { get; set; }

        public CustomerViewModel(Customer customer, RenderContext context)
        {
            this.context = context;
            this.customer = customer;
        }

        public string Telephone => customer.Telephone;

        public string EmailAddress => customer.EmailAddress;

        public string FirstName => customer.FirstName;

        public string LastName => customer.LastName;

    }
}

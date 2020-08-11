using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.Request
{
    public class CustomerRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Telephone { get; set; }
    }
}

using Kooboo.Sites.Ecommerce.Promotion;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.ViewModel
{
    public class CustomerForPageViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public long MembershipNumber { get; set; }

        public string EmailAddress { get; set; }

        public string Telephone { get; set; }
    }
}

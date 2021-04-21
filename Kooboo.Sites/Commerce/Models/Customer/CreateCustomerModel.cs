using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Customer
{
    public class CreateCustomerModel
    {
        private string userName;
        private string password;
        private string email;
        private string phone;

        public string UserName { get => userName?.Trim(); set => userName = value; }
        public string Password { get => password?.Trim(); set => password = value; }
        public string Email { get => email?.Trim(); set => email = value; }
        public string Phone { get => phone?.Trim(); set => phone = value; }
    }
}

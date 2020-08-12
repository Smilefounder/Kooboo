using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.Request;
using Kooboo.Sites.Ecommerce.Service;
using System.Collections.Generic;
using System.ComponentModel;

namespace Kooboo.Sites.Ecommerce.KScript
{
    public class KCustomer
    {
        private RenderContext context { get; set; }

        private ICustomerService service { get; set; }

        public KCustomer(RenderContext context)
        {
            this.context = context;
            this.service = ServiceProvider.Customer(this.context);
        }

        [Description(@"<script engine=""kscript"">
    var customer = k.commerce.customer.creatAccount({
		userName: ""kooboo"",
        email:  ""admin@kooboo.cn "",
        password:  ""123456 "",
        firstName:  ""kooboo "",
        lastName:  ""CN "",
        telephone:  ""18000000000""
    });
	//k.response.write(customer);
</script>")]
        [KDefineType(Params = new[] { typeof(CustomerRequest) })]
        public Customer CreatAccount(IDictionary<string, object> request)
        {
            var data = Kooboo.Lib.Reflection.TypeHelper.ToObject<CustomerRequest>(request);
            return service.CreatAccount(data.UserName, data.Email, data.Password, data.FirstName, data.LastName, data.Telephone);
        }

        public Customer Login(string nameOrEmail, string password)
        {
            return service.Login(nameOrEmail, password);
        }

        public bool Logout()
        {
            return service.Logout();
        }

        public bool IsUserNameAvailable(string username)
        {
            return service.IsUSerNameAvailable(username);
        }

        public bool IsEmailAddressAvailable(string emailaddress)
        {
            return service.IsEmailAddressAvailable(emailaddress);
        }

        private Customer _customer;
        public Customer Current
        {
            get
            {
                if (_customer == null)
                {
                    _customer = service.GetFromContext(this.context);
                }
                return _customer;
            }
        }
    }
}

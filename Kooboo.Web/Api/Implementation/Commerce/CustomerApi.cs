using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Sites.Commerce.Models.Customer;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class CustomerApi : CommerceApi
    {
        public override string ModelName => "Customer";

        public void Register(CreateCustomerModel model, ApiCall apiCall)
        {
            GetService<CustomerService>(apiCall).Register(model);
        }

        public PagedListModel<CustomerListModel> List(PagingQueryModel viewModel, ApiCall apiCall)
        {
            return GetService<CustomerService>(apiCall).List(viewModel);
        }
    }
}

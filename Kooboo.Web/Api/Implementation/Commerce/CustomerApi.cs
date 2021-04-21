using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Sites.Commerce.Models.Customer;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class CustomerApi : IApi
    {
        public string ModelName => "Customer";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public void Register(CreateCustomerModel model, ApiCall apiCall)
        {
            new CustomerService(apiCall.Context).Register(model);
        }

        public PagedListModel<CustomerListModel> List(PagingQueryModel viewModel, ApiCall apiCall)
        {
            return new CustomerService(apiCall.Context).List(viewModel);
        }
    }
}

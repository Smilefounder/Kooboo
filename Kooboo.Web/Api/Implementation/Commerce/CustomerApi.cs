using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class CustomerApi : IApi
    {
        public string ModelName => "Customer";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public void Register(string userName, string password, ApiCall apiCall)
        {
            new CustomerService(apiCall.Context).Register(userName, password);
        }

        public PagedListModel<CustomerModel> List(PagingQueryModel viewModel, ApiCall apiCall)
        {
            return new CustomerService(apiCall.Context).List(viewModel);
        }
    }
}

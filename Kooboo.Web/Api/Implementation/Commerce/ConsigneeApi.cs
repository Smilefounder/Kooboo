using Kooboo.Api;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class ConsigneeApi : IApi
    {
        public string ModelName => "Consignee";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public Consignee Get(Guid id, ApiCall apiCall)
        {
            return new ConsigneeService(apiCall.Context).Get(id);
        }

        public void Post(Consignee consignee, ApiCall apiCall)
        {
            new ConsigneeService(apiCall.Context).Save(consignee);
        }

        public void Delete(Guid id, ApiCall apiCall)
        {
            new ConsigneeService(apiCall.Context).Delete(id);
        }

        public Consignee[] List(Guid id, ApiCall apiCall)
        {
            return new ConsigneeService(apiCall.Context).List(id);
        }
    }
}

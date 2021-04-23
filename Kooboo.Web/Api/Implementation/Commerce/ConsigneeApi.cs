using Kooboo.Api;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class ConsigneeApi : CommerceApi
    {
        public override string ModelName => "Consignee";

        public Consignee Get(Guid id, ApiCall apiCall)
        {
            return GetService<ConsigneeService>(apiCall).Get(id);
        }

        public void Post(Consignee consignee, ApiCall apiCall)
        {
            GetService<ConsigneeService>(apiCall).Save(consignee);
        }

        public void Delete(Guid id, ApiCall apiCall)
        {
            GetService<ConsigneeService>(apiCall).Delete(id);
        }

        public Consignee[] List(Guid id, ApiCall apiCall)
        {
            return GetService<ConsigneeService>(apiCall).List(id);
        }
    }
}

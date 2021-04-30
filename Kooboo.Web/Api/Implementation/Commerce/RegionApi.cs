using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class RegionApi : CommerceApi
    {
        public override string ModelName => "Region";

        public object[] Countries(ApiCall apiCall)
        {
            return GetService<RegionService>(apiCall).GetCountries();
        }

        public object[] Provinces(ApiCall apiCall, string code)
        {
            return GetService<RegionService>(apiCall).GetProvinces(code);
        }
    }
}

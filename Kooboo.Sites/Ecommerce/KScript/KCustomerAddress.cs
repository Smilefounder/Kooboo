using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Ecommerce.Service;
using Kooboo.Sites.Ecommerce.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Kooboo.Sites.Ecommerce.KScript
{
    public class KCustomerAddress
    {
        private RenderContext context { get; set; }

        private CustomerAddressService service { get; set; }

        public KCustomerAddress(RenderContext context)
        {
            this.context = context;
            this.service = Kooboo.Sites.Ecommerce.ServiceProvider.GetService<CustomerAddressService>(this.context);
        }

        [Description("GetCountries")]
        public List<GeographicalRegionViewModel> GetCountries()
        {
            return this.service.GetCountries();
        }

        /// <summary>
        /// parentId
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [Description("Get Children Geographical Regions")]
        public List<GeographicalRegionViewModel> GetChildrenGeographicalRegions(string id)
        {
            return this.service.GetChildrenGeographicalRegions(id);
        }
    }
}

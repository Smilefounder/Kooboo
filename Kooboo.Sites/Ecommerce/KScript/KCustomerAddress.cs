using Kooboo.Data.Context;
using Kooboo.Sites.Ecommerce.Service;
using Kooboo.Sites.Ecommerce.ViewModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Kooboo.Sites.Ecommerce.KScript
{
    public class KCustomerAddress
    {
        private RenderContext context { get; set; }

        private CustomerAddressService service { get; set; }

        private GeographicalRegionService geoService { get; set; }

        public KCustomerAddress(RenderContext context)
        {
            this.context = context;
            this.service = ServiceProvider.GetService<CustomerAddressService>(this.context);
            this.geoService = ServiceProvider.GetService<GeographicalRegionService>(this.context);
        }

        [Description("GetCountries")]
        public List<GeographicalRegionViewModel> GetCountries()
        {
            return this.geoService.GetCountries();
        }

        /// <summary>
        /// parentId
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [Description("Get Children Geographical Regions")]
        public List<GeographicalRegionViewModel> GetChildrenGeographicalRegions(string id)
        {
            return this.geoService.GetChildrenGeographicalRegions(id);
        }
    }
}

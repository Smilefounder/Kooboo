using Kooboo.Data.Context;
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
        public List<GeoCountryViewModel> GetCountries()
        {
            return this.service.GetCountries();
        }
    }
}

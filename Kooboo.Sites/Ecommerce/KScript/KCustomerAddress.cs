using Kooboo.Data.Context;
using Kooboo.Sites.Ecommerce.Models;
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

        [Description("Add Customer Address")]
        public bool AddCustomerAddress(string selectedIdsStr, string detailAdress, string postCode, string consignee, string contactNumber)
        {
            var geoAdress = new List<string>();
            var selectIds = selectedIdsStr.Split(',').ToList();
            foreach (var item in selectIds)
            {
                geoAdress.Add(this.geoService.Get(item).Name);
            }

            var toAdd = new CustomerAddress
            {
                Country = geoAdress.FirstOrDefault(),
                Address = string.Join(" ", geoAdress) + "\r\n" + detailAdress,
                PostCode = postCode,
                Consignee = consignee,
                ContactNumber = contactNumber,
                CustomerId = this.service.CommerceContext.customer.Id
            };

            return this.service.AddOrUpdate(toAdd, this.service.CommerceContext.customer.Id);
        }

        public List<CustomerAddress> GetAllCustomerAddresses()
        {
            return this.service.Repo.Query.Where(it => it.CustomerId == this.service.CommerceContext.customer.Id).SelectAll();
        }
    }
}

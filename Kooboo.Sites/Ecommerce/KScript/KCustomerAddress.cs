using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.Request;
using Kooboo.Sites.Ecommerce.Service;
using Kooboo.Sites.Ecommerce.ViewModel;
using System;
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
        [KDefineType(Params = new[] { typeof(CustomerAddressRequest) })]
        public bool AddCustomerAddress(IDictionary<string, object> request)
        {
            var requestData = Lib.Reflection.TypeHelper.ToObject<CustomerAddressRequest>(request);

            var toAdd = new CustomerAddress
            {
                Country = requestData.CountryName,
                City = requestData.ProvinceName,
                Address = requestData.Address1,
                Address2 = requestData.Address2,
                PostCode = requestData.PostCode,
                Consignee = requestData.ConsigneeName,
                ContactNumber = requestData.ContactNumber,
                CustomerId = this.service.CommerceContext.customer.Id
            };

            return this.service.AddOrUpdate(toAdd);
        }

        [Description("Update Customer Address")]
        public bool UpdateCustomerAddress(string customerAddressId, string selectedIdsStr, string detailAdress, string postCode, string consignee, string contactNumber)
        {
            List<string> geoAdresses = GetgeoAdresses(selectedIdsStr);
            var toUpdate = this.service.Get(customerAddressId);
            if (toUpdate == null)
            {
                return false;
            }

            toUpdate.Country = geoAdresses.FirstOrDefault();
            toUpdate.Address = MapAddress(detailAdress, geoAdresses);
            toUpdate.PostCode = postCode;
            toUpdate.Consignee = consignee;
            toUpdate.ContactNumber = contactNumber;
            toUpdate.CustomerId = this.service.CommerceContext.customer.Id;

            return this.service.AddOrUpdate(toUpdate);
        }

        private string MapAddress(string detailAdress, List<string> geoAdresses)
        {
            return string.Join(" ", geoAdresses) + "\r\n" + detailAdress;
        }

        [Description("Get All Customer Address")]
        public CustomerAddressViewModel[] GetAllCustomerAddresses()
        {
            var allCustomerAddresses = this.service.Repo.Query.Where(it => it.CustomerId == this.service.CommerceContext.customer.Id).SelectAll();
            if (allCustomerAddresses != null)
            {
                return allCustomerAddresses.OrderByDescending(it => it.CreationDate).Select(it => new CustomerAddressViewModel(it)).ToArray();
            }
            return null;
        }

        [Description("Get Customer Address")]
        public CustomerAddressViewModel GetCustomerAddress(string customerAddressId)
        {
            Guid id;
            if (Guid.TryParse(customerAddressId, out id))
            {
                var customerAddress = this.service.Get(id);
                if (customerAddress != null)
                {
                    return new CustomerAddressViewModel(customerAddress);
                }
            }

            return null;
        }

        [Description("Delete All Customer Address")]
        public void DeleteCustomerAddress(string customerAddressId)
        {
            Guid id;
            if (Guid.TryParse(customerAddressId, out id))
            {
                this.service.Delete(id);
            }
        }

        private List<string> GetgeoAdresses(string selectedIdsStr)
        {
            var geoAdress = new List<string>();
            if (string.IsNullOrEmpty(selectedIdsStr))
            {
                return geoAdress;
            }

            var selectIds = selectedIdsStr.Split(',').ToList();
            foreach (var item in selectIds)
            {
                geoAdress.Add(this.geoService.Get(item).Name);
            }

            return geoAdress;
        }
    }
}

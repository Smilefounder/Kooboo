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

        private ICustomerService customerService { get; set; }

        public KCustomerAddress(RenderContext context)
        {
            this.context = context;
            this.service = ServiceProvider.GetService<CustomerAddressService>(this.context);
            this.geoService = ServiceProvider.GetService<GeographicalRegionService>(this.context);
            this.customerService = ServiceProvider.Customer(this.context);
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
            var customer = this.service.CommerceContext.customer;
            var toAdd = new CustomerAddress
            {
                Country = requestData.Country,
                City = requestData.City,
                Address = requestData.Address,
                Address2 = requestData.Address2,
                PostCode = requestData.PostCode,
                Consignee = requestData.Consignee,
                ContactNumber = requestData.ContactNumber,
                HouseNumber = requestData.HouseNumber,
                CustomerId = customer.Id
            };

            if (requestData.IsDefault)
            {
                customer.DefaultShippingAddress = toAdd.Id;
                this.customerService.AddOrUpdate(customer);
            }
            return this.service.AddOrUpdate(toAdd);
        }

        [Description("Update Customer Address")]
        [KDefineType(Params = new[] { typeof(string), typeof(CustomerAddressRequest) })]
        public bool UpdateCustomerAddress(string addressId, IDictionary<string, object> request)
        {
            if (!Guid.TryParse(addressId, out var id))
            {
                return false;
            }
            var toUpdate = this.service.Get(id);
            if (toUpdate == null)
            {
                return false;
            }
            var requestData = Lib.Reflection.TypeHelper.ToObject<CustomerAddressRequest>(request);
            toUpdate.Country = requestData.Country;
            toUpdate.City = requestData.City;
            toUpdate.Address = requestData.Address;
            toUpdate.Address2 = requestData.Address2;
            toUpdate.PostCode = requestData.PostCode;
            toUpdate.Consignee = requestData.Consignee;
            toUpdate.ContactNumber = requestData.ContactNumber;
            toUpdate.HouseNumber = requestData.HouseNumber;

            if (requestData.IsDefault)
            {
                var customer = this.service.CommerceContext.customer;
                customer.DefaultShippingAddress = id;
                this.customerService.AddOrUpdate(customer);
            }
            return this.service.AddOrUpdate(toUpdate);
        }

        [Description("Get All Customer Address")]
        public CustomerAddress[] GetAllCustomerAddresses()
        {
            var allCustomerAddresses = this.service.Repo.Query.Where(it => it.CustomerId == this.service.CommerceContext.customer.Id).SelectAll();
            if (allCustomerAddresses != null)
            {
                return allCustomerAddresses.OrderByDescending(it => it.CreationDate).ToArray();
            }
            return null;
        }

        [Description("Get Customer Address")]
        public CustomerAddress GetCustomerAddress(string customerAddressId)
        {
            Guid id;
            if (Guid.TryParse(customerAddressId, out id))
            {
                return this.service.Get(id);
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
    }
}

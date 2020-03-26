using Kooboo.Sites.Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Kooboo.Sites.Ecommerce.ViewModel;
using Kooboo.Lib.Helper;
using Newtonsoft.Json;

namespace Kooboo.Sites.Ecommerce.Service
{
    public class CustomerAddressService : ServiceBase<CustomerAddress>
    {
        public List<GeographicalRegionViewModel> GetCountries()
        {
            var geoServer = ServiceProvider.GeographicalRegion(this.Context);
            var allCountries = geoServer.Repo.Query.Where(it => it.ParentId == Guid.Empty).SelectAll();
            if (allCountries.Count != 0)
            {
                return MapCountries(allCountries);
            }

            var queryUrl = "http://api.geonames.org/countryInfoJSON?username=tenghui";
            var deserializeResult = JsonConvert.DeserializeObject<GeoCountryResponseModel>(ApiClient.Create().GetAsync(queryUrl).Result.Content);
            if (deserializeResult == null)
            {
                return new List<GeographicalRegionViewModel>();
            }

            SaveAllGeoCountries(deserializeResult);

            return MapCountries(geoServer.Repo.Query.Where(it => it.ParentId == Guid.Empty).SelectAll());
        }

        public List<GeographicalRegionViewModel> GetChildrenGeographicalRegions(string id)
        {
            var geoServer = ServiceProvider.GeographicalRegion(this.Context);

            Guid parentId;
            if (!Guid.TryParse(id, out parentId))
            {
                return new List<GeographicalRegionViewModel>();
            }

            var childrenGeos = geoServer.Repo.Query.Where(it => it.ParentId == parentId).SelectAll();
            if (childrenGeos.Count != 0)
            {
                return MapCountries(childrenGeos);
            }

            var parentGeo = geoServer.Get(parentId);
            var queryUrl = "http://api.geonames.org/childrenJSON?username=tenghui&geonameId=" + parentGeo.GeoNameId;
            var deserializeResult = JsonConvert.DeserializeObject<ChildrenGeoResponseModel>(ApiClient.Create().GetAsync(queryUrl).Result.Content);

            if (deserializeResult.TotalResultsCount != 0)
            {
                foreach (var item in deserializeResult.ChildrenGeoInfo)
                {
                    var geographicalRegion = new GeographicalRegion { Name = item.GeoToponymName, GeoNameId = item.GeoNameId, ParentId = parentGeo.Id };
                    ServiceProvider.GeographicalRegion(this.Context).AddOrUpdate(geographicalRegion);
                }
            }

            return MapCountries(geoServer.Repo.Query.Where(it => it.ParentId == parentId).SelectAll());
        }

        private void SaveAllGeoCountries(GeoCountryResponseModel deserializeResult)
        {
            foreach (var item in deserializeResult.GeoCountryInfo)
            {
                var geoCountry = new GeographicalRegion { Name = item.GeoCountryName, GeoNameId = item.GeoNameId, ParentId = Guid.Empty };
                ServiceProvider.GeographicalRegion(this.Context).AddOrUpdate(geoCountry);
            }
        }

        private static List<GeographicalRegionViewModel> MapCountries(List<GeographicalRegion> allGeographicalRegions)
        {
            return allGeographicalRegions.Select(it => new GeographicalRegionViewModel
            {
                Id = it.Id,
                GeoNameId = it.GeoNameId,
                GeoName = it.Name
            }).ToList();
        }
    }
}

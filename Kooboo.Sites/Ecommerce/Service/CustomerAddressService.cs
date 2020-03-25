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
        public List<GeoCountryViewModel> GetCountries()
        {
            var resultCountries = new List<GeoCountryViewModel>();

            var allCountries = ServiceProvider.GeoCountry(this.Context).Repo.List();
            if (allCountries.Count != 0)
            {
                return MapCountries(allCountries);
            }

            var queryUrl = "http://api.geonames.org/countryInfoJSON?username=tenghui";
            var deserializeResult = JsonConvert.DeserializeObject<GeoCountryResponseModel>(ApiClient.Create().GetAsync(queryUrl).Result.Content);
            if (deserializeResult == null)
            {
                return resultCountries;
            }

            SaveAllGeoCountries(deserializeResult);

            return MapCountries(ServiceProvider.GeoCountry(this.Context).Repo.List());
        }

        private void SaveAllGeoCountries(GeoCountryResponseModel deserializeResult)
        {
            var geoCountryService = ServiceProvider.GeoCountry(this.Context);
            foreach (var item in deserializeResult.GeoCountryInfo)
            {
                var geoCountry = new GeoCountry { Name = item.GeoCountryName, GeoNameId = item.GeoNameId, CurrencyCode = item.CurrencyCode, CountryCode = item.CountryCode, ContinentName = item.ContinentName, Capital = item.Capital };
                geoCountryService.AddOrUpdate(geoCountry);
            }
        }

        private static List<GeoCountryViewModel> MapCountries(List<GeoCountry> allCountries)
        {
            return allCountries.Select(it => new GeoCountryViewModel
            {
                Id = it.Id,
                GeoNameId = it.GeoNameId,
                GeoCountryName = it.Name
            }).ToList();
        }
    }
}

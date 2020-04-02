using Kooboo.Lib.Helper;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Sites.Ecommerce.Service
{
    public class GeographicalRegionService : ServiceBase<GeographicalRegion>
    {
        // 目前只做save，没有比对update
        public List<GeographicalRegionViewModel> GetCountries()
        {
            var allCountries = this.Repo.Query.Where(it => it.ParentId == Guid.Empty).SelectAll();
            if (allCountries.Count != 0)
            {
                return MapGeographicalRegions(allCountries);
            }

            var queryUrl = "http://api.geonames.org/countryInfoJSON?username=tenghui_kooboo";
            var deserializeResult = JsonConvert.DeserializeObject<GeoCountryResponseModel>(ApiClient.Create().GetAsync(queryUrl).Result.Content);
            if (deserializeResult == null)
            {
                return new List<GeographicalRegionViewModel>();
            }

            SaveAllGeoCountries(deserializeResult);

            return MapGeographicalRegions(this.Repo.Query.Where(it => it.ParentId == Guid.Empty).SelectAll());
        }

        public List<GeographicalRegionViewModel> GetChildrenGeographicalRegions(string id)
        {
            Guid parentId;
            if (!Guid.TryParse(id, out parentId))
            {
                return new List<GeographicalRegionViewModel>();
            }

            var childrenGeos = this.Repo.Query.Where(it => it.ParentId == parentId).SelectAll();
            if (childrenGeos.Count != 0)
            {
                return MapGeographicalRegions(childrenGeos);
            }

            var parentGeo = this.Get(parentId);
            var queryUrl = "http://api.geonames.org/childrenJSON?username=tenghui_kooboo&maxRows=10000&geonameId=" + parentGeo.GeoNameId;
            var deserializeResult = JsonConvert.DeserializeObject<ChildrenGeoResponseModel>(ApiClient.Create().GetAsync(queryUrl).Result.Content);

            if (deserializeResult.TotalResultsCount != 0)
            {
                foreach (var item in deserializeResult.ChildrenGeoInfo)
                {
                    var geographicalRegion = new GeographicalRegion { Name = item.GeoToponymName, GeoNameId = item.GeoNameId, ParentId = parentGeo.Id };
                    this.AddOrUpdate(geographicalRegion);
                }
            }

            return MapGeographicalRegions(this.Repo.Query.Where(it => it.ParentId == parentId).SelectAll());
        }

        private void SaveAllGeoCountries(GeoCountryResponseModel deserializeResult)
        {
            foreach (var item in deserializeResult.GeoCountryInfo)
            {
                var geoCountry = new GeographicalRegion { Name = item.GeoCountryName, GeoNameId = item.GeoNameId, ParentId = Guid.Empty };
                this.AddOrUpdate(geoCountry);
            }
        }

        private static List<GeographicalRegionViewModel> MapGeographicalRegions(List<GeographicalRegion> allGeographicalRegions)
        {
            allGeographicalRegions = allGeographicalRegions.OrderBy(it => it.Name).ToList();
            return allGeographicalRegions.Select(it => new GeographicalRegionViewModel
            {
                Id = it.Id,
                GeoNameId = it.GeoNameId,
                GeoName = it.Name
            }).ToList();
        }
    }
}

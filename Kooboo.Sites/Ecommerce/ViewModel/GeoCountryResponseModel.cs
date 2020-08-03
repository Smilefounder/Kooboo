using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.ViewModel
{
    public class GeoCountryResponseModel
    {
        [JsonProperty("geonames")]
        public List<GeoCountryInfo> GeoCountryInfo { get; set; }
    }

    public class GeoCountryInfo
    {
        [JsonProperty("countryName")]
        public string GeoCountryName { get; set; }

        [JsonProperty("geonameId")]
        public string GeoNameId { get; set; }
    }
}

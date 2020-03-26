using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.ViewModel
{
    public class ChildrenGeoResponseModel
    {
        [JsonProperty("totalResultsCount")]
        public int TotalResultsCount { get; set; }

        [JsonProperty("geonames")]
        public List<ChildrenGeo> ChildrenGeoInfo { get; set; }
    }

    public class ChildrenGeo
    {
        [JsonProperty("toponymName")]
        public string GeoToponymName { get; set; }

        [JsonProperty("geonameId")]
        public string GeoNameId { get; set; }
    }
}

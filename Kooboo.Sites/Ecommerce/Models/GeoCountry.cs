using Kooboo.Sites.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.Models
{
    [Kooboo.Attributes.Diskable(Kooboo.Attributes.DiskType.Json)]
    public class GeoCountry : CoreObject
    {
        public string GeoNameId { get; set; }

        public string CurrencyCode { get; set; }

        public string CountryCode { get; set; }

        public string ContinentName { get; set; }

        public string Capital { get; set; }
    }
}

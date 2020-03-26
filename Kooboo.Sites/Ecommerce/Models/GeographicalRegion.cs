using Kooboo.Sites.Models;
using System;

namespace Kooboo.Sites.Ecommerce.Models
{
    [Attributes.Diskable(Attributes.DiskType.Json)]
    public class GeographicalRegion : CoreObject
    {
        public string GeoNameId { get; set; }

        public Guid ParentId { get; set; }
    }
}

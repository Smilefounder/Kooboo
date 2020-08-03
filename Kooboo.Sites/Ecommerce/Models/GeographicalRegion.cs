using Kooboo.Sites.Models;
using System;

namespace Kooboo.Sites.Ecommerce.Models
{
    [Attributes.Diskable(Attributes.DiskType.Json)]
    public class GeographicalRegion : CoreObject
    {
        private Guid _id;
        public override Guid Id
        {
            get => _id == Guid.Empty ? (_id = System.Guid.NewGuid()) : _id;
            set => _id = value;
        }

        public string GeoNameId { get; set; }

        public Guid ParentId { get; set; }
    }
}

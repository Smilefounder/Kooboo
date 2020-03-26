using Kooboo.IndexedDB;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Repository;

namespace Kooboo.Sites.Ecommerce.Repository
{
    public class GeographicalRegionRepository : SiteRepositoryBase<GeographicalRegion>
    {
        public override ObjectStoreParameters StoreParameters
        {
            get
            {
                ObjectStoreParameters para = new ObjectStoreParameters();
                para.AddIndex<GeographicalRegion>(o => o.Name);
                para.AddColumn<GeographicalRegion>(o => o.ParentId);
                return para;
            }
        }
    }
}

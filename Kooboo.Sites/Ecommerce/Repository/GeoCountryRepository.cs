using Kooboo.IndexedDB;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Repository;

namespace Kooboo.Sites.Ecommerce.Repository
{
    public class GeoCountryRepository : SiteRepositoryBase<GeoCountry>
    {
        public override ObjectStoreParameters StoreParameters
        {
            get
            {
                ObjectStoreParameters para = new ObjectStoreParameters();
                para.AddIndex<GeoCountry>(o => o.Name);
                para.AddColumn<GeoCountry>(o => o.CountryCode);
                return para;
            }
        }
    }
}

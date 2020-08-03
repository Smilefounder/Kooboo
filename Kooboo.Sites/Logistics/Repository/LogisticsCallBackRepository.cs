using Kooboo.IndexedDB;
using Kooboo.Sites.Logistics.Models;

namespace Kooboo.Sites.Logistics.Repository
{
    public class LogisticsCallBackRepository : Kooboo.Sites.Repository.SiteRepositoryBase<LogisticsCallback>
    {
        public override ObjectStoreParameters StoreParameters
        {
            get
            {
                ObjectStoreParameters para = new ObjectStoreParameters();
                para.AddColumn<LogisticsCallback>(o => o.RequestId);
                para.SetPrimaryKeyField<LogisticsCallback>(o => o.Id);
                return para;
            }
        }
    }
}

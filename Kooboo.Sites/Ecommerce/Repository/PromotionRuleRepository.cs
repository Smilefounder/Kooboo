using Kooboo.IndexedDB;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Repository;

namespace Kooboo.Sites.Ecommerce.Repository
{

    public class PromotionRuleRepository : SiteRepositoryBase<PromotionRule>
    {
        public override ObjectStoreParameters StoreParameters
        {
            get
            {
                ObjectStoreParameters paras = new ObjectStoreParameters();
                paras.AddIndex<PromotionRule>(it => it.Name);
                paras.SetPrimaryKeyField<ProductVariants>(o => o.Id);
                return paras;
            }
        }

    }






}

using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Sites.Commerce.Models;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class PromotionApi : CommerceApi
    {
        public override string ModelName => "Promotion";

        public void Post(PromotionModel model, ApiCall apiCall)
        {
            GetService<PromotionService>(apiCall).Save(model);
        }

        public PagedListModel<PromotionListModel> List(ApiCall apiCall, PagingQueryModel model)
        {
            return GetService<PromotionService>(apiCall).List(model);
        }

        public PromotionModel Get(ApiCall apiCall, Guid id)
        {
            return GetService<PromotionService>(apiCall).Get(id);
        }

        public void Deletes(Guid[] ids, ApiCall apiCall)
        {
            GetService<PromotionService>(apiCall).Deletes(ids);
        }
    }
}

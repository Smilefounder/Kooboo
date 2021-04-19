using Kooboo.Api;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Sites.Commerce.Models;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class PromotionApi : IApi
    {
        public string ModelName => "Promotion";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public void Post(PromotionModel model, ApiCall apiCall)
        {
            new PromotionService(apiCall.Context).Save(model);
        }

        public PagedListModel<PromotionListModel> List(ApiCall apiCall, PagingQueryModel model)
        {
            return new PromotionService(apiCall.Context).List(model);
        }

        public PromotionModel Get(ApiCall apiCall, Guid id)
        {
            return new PromotionService(apiCall.Context).Get(id);
        }

        public void Deletes(Guid[] ids, ApiCall apiCall)
        {
            new PromotionService(apiCall.Context).Deletes(ids);
        }
    }
}

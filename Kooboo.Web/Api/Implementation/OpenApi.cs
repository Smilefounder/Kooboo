using Kooboo.Api;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.OpenApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation
{
    public class OpenApi : SiteObjectApi<Sites.Models.OpenApi>
    {
        public override Guid Post(ApiCall call)
        {
            var model = JsonHelper.Deserialize<Sites.Models.OpenApi>(call.Context.Request.Body);
            model = Helpers.GetOrValidOpenApiDoc(model);
            call.Context.WebSite.SiteDb().OpenApi.AddOrUpdate(model);
            return model.Id;
        }
    }
}

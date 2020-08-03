using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data.Context;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Logistics.Models;
using Kooboo.Sites.Logistics.Repository;

namespace Kooboo.Sites.Logistics
{
    public class LogiticsCallbackWorker : ILogiticsCallbackWorker
    {
        public void Process(LogisticsCallback callback, RenderContext context)
        {
            if (context.WebSite != null && callback.RequestId != default(Guid))
            {
                var sitedb = context.WebSite.SiteDb();
                // fire the code first...
                var callbackrepo = sitedb.GetSiteRepository<LogisticsCallBackRepository>();

                callbackrepo.AddOrUpdate(callback);
            }
        }
    }
}

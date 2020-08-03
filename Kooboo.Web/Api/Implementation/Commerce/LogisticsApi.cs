using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Api;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Logistics;
using Kooboo.Sites.Logistics.Models;
using Kooboo.Sites.Logistics.Repository;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class LogisticsApi : IApi
    {
        public string ModelName => "Logistics";

        public bool RequireSite => false;

        public bool RequireUser => false;

        public LogisticsStatusResponse CheckStatus(ApiCall call)
        {
            var id = call.GetValue("logisticsrequestid", "refid", "referenceId", "requestid", "id");

            if (!string.IsNullOrEmpty(id))
            {
                var repo = call.Context.WebSite.SiteDb().GetSiteRepository<LogisticsRequestRepository>();

                var request = repo.Get(id);
                if (request != null)
                {
                    return LogisticsManager.EnquireStatus(request, call.Context);
                }
            }
            return new LogisticsStatusResponse() { Message = "No result" };
        }

        public string EnquirePostage(ApiCall call)
        {
            var id = call.GetValue("logisticsrequestid", "refid", "referenceId", "requestid", "id");

            if (!string.IsNullOrEmpty(id))
            {
                var repo = call.Context.WebSite.SiteDb().GetSiteRepository<LogisticsRequestRepository>();

                var request = repo.Get(id);
                if (request != null)
                {
                    return LogisticsManager.EnquirePostage(request, call.Context);
                }
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data.Context;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Logistics.Models;

namespace Kooboo.Sites.Logistics
{
    public static class LogisticsManager
    {
        public static ILogisticsMethod GetMethod(string logisticsmethod)
        {
            logisticsmethod = logisticsmethod.ToLower();

            foreach (var item in LogisticsContainer.LogisticsMethods)
            {
                if (item.Name.ToLower() == logisticsmethod)
                {
                    return item;
                }
            }
            return null;
        }

        public static ILogisticsMethod GetMethod(string MethodName, RenderContext context)
        {
            if (MethodName == null)
            {
                return null;
            }

            var method = GetMethod(MethodName);

            if (method != null)
            {
                var methodType = method.GetType();

                var sitedb = context.WebSite.SiteDb();

                var logisticsMethod = Activator.CreateInstance(methodType) as ILogisticsMethod;
                logisticsMethod.Context = context;

                if (Lib.Reflection.TypeHelper.HasGenericInterface(methodType, typeof(ILogisticsMethod<>)))
                {
                    var settingtype = Lib.Reflection.TypeHelper.GetGenericType(methodType);

                    if (settingtype != null)
                    {
                        var settingvalue = sitedb.CoreSetting.GetSiteSetting(settingtype) as ILogisticsMethod;
                        //Setting
                        var setter = Lib.Reflection.TypeHelper.GetSetObjectValue("Setting", methodType, settingtype);
                        setter(logisticsMethod, settingvalue);

                        return logisticsMethod;
                    }
                    else
                    {
                        throw new Exception(MethodName + " missing setting infomatoin");
                    }
                }

            }

            else

            {
                throw new Exception(MethodName + " not found");
            }

            throw new Exception(MethodName + " missing setting infomatoin");
        }

        public static LogisticsRequest GetRequest(Guid LogisticsRequestId, RenderContext context)
        {
            if (context.WebSite != null)
            {
                var sitedb = context.WebSite.SiteDb();
                var repo = sitedb.GetSiteRepository<Repository.LogisticsRequestRepository>();

                var result = repo.Get(LogisticsRequestId);

                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public static LogisticsStatusResponse EnquireStatus(LogisticsRequest Request, RenderContext context)
        {
            var logisticsmethod = LogisticsManager.GetMethod(Request.LogisticsMethod, context);
            if (logisticsmethod != null)
            {
                logisticsmethod.Context = context;

                var status = logisticsmethod.checkStatus(Request);

                return status;
            }
            return new LogisticsStatusResponse();
        }
    }
}

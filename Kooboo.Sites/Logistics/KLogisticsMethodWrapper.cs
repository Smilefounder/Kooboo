using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.IndexedDB.Dynamic;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Logistics.Models;

namespace Kooboo.Sites.Logistics
{
    public class KLogisticsMethodWrapper
    {
        private RenderContext Context { get; set; }

        private ILogisticsMethod LogisticsMethod { get; set; }

        public KLogisticsMethodWrapper(ILogisticsMethod logisticsMethod, RenderContext context)
        {
            this.LogisticsMethod = logisticsMethod;
            this.Context = context;
        }

        public ILogisticsResponse CreateOrder(object value)
        {
            var request = ParseRequest(value);

            var sitedb = this.Context.WebSite.SiteDb();

            var repo = sitedb.GetSiteRepository<Repository.LogisticsRequestRepository>();
            repo.AddOrUpdate(request);

            var result = this.LogisticsMethod.CreateOrder(request);

            if (!string.IsNullOrWhiteSpace(result.logisticsMethodReferenceId))
            {
                request.ReferenceId = result.logisticsMethodReferenceId;
                request.Created = true;
            }
            else
            {
                request.Failed = true;
            }

            if (!string.IsNullOrWhiteSpace(request.ReferenceId))
            {
                repo.AddOrUpdate(request);
            }

            return result;
        }

        public LogisticsStatusResponse checkStatus(object requestId)
        {
            if (requestId == null)
            {
                string strid = requestId.ToString();
                Guid id;
                if (System.Guid.TryParse(strid, out id))
                {
                    var request = LogisticsManager.GetRequest(id, Context);

                    if (request != null)
                    {
                        var status = this.LogisticsMethod.checkStatus(request);
                        if (status != null)
                        {
                            LogisticsManager.CallBack(new LogisticsCallback() { RequestId = request.Id, Status = status.Status, ResponseMessage = "kscript check status" }, this.Context);
                        }
                    }
                }
            }

            return new LogisticsStatusResponse();
        }


        public string GetPostage(object value)
        {
            var request = ParsePostageRequest(value);

            var sitedb = this.Context.WebSite.SiteDb();

            var repo = sitedb.GetSiteRepository<Repository.LogisticsRequestRepository>();
            repo.AddOrUpdate(request);

            var result = this.LogisticsMethod.GetPostage(request);

            if (!string.IsNullOrWhiteSpace(result))
            {
                request.Postage = result;
                repo.AddOrUpdate(request);
            }

            return result;
        }

        [KIgnore]
        public LogisticsRequest ParsePostageRequest(object dataobj)
        {
            Dictionary<string, object> additionals = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            LogisticsRequest request = new LogisticsRequest();

            System.Collections.IDictionary idict = dataobj as System.Collections.IDictionary;

            IDictionary<string, object> dynamicobj = null;

            if (idict == null)
            {
                dynamicobj = dataobj as IDictionary<string, object>;
                foreach (var item in dynamicobj)
                {
                    additionals[item.Key] = item.Value;
                }
            }
            else
            {
                foreach (var item in idict.Keys)
                {
                    if (item != null)
                    {
                        additionals[item.ToString()] = idict[item];
                    }
                }
            }

            request.Additional = additionals;


            var id = GetValue<string>(idict, dynamicobj, "id", "requestId", "logisticsrequestid");
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (Guid.TryParse(id, out Guid requestid))
                {
                    request.Id = requestid;
                }
            }

            var sendInfo = new Info
            {
                Prov = GetValue<string>(idict, dynamicobj, "senderprovince")
            };

            var receiverInfo = new Info
            {
                Prov = GetValue<string>(idict, dynamicobj, "receiverprovince")
            };

            request.SenderInfo = sendInfo;
            request.ReceiverInfo = receiverInfo;
            request.Height = GetValue<double>(idict, dynamicobj, "cargoheight");
            request.Weight = GetValue<double>(idict, dynamicobj, "cargoweight");
            request.Length = GetValue<double>(idict, dynamicobj, "cargolength");
            if (this.LogisticsMethod != null)
            {
                request.LogisticsMethod = LogisticsMethod.Name;
            }

            return request;
        }

        [KIgnore]
        public LogisticsRequest ParseRequest(object dataobj)
        {
            Dictionary<string, object> additionals = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            LogisticsRequest request = new LogisticsRequest();

            System.Collections.IDictionary idict = dataobj as System.Collections.IDictionary;

            IDictionary<string, object> dynamicobj = null;

            if (idict == null)
            {
                dynamicobj = dataobj as IDictionary<string, object>;
                foreach (var item in dynamicobj)
                {
                    additionals[item.Key] = item.Value;
                }
            }
            else
            {
                foreach (var item in idict.Keys)
                {
                    if (item != null)
                    {
                        additionals[item.ToString()] = idict[item];
                    }
                }
            }

            request.Additional = additionals;


            var id = GetValue<string>(idict, dynamicobj, "id", "requestId", "logisticsrequestid");
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (Guid.TryParse(id, out Guid requestid))
                {
                    request.Id = requestid;
                }
            }

            request.SenderInfo.Address = GetValue<string>(idict, dynamicobj, "senderaddress");
            request.SenderInfo.City = GetValue<string>(idict, dynamicobj, "sendercity");
            request.SenderInfo.County = GetValue<string>(idict, dynamicobj, "sendercountry");
            request.SenderInfo.Mobile = GetValue<string>(idict, dynamicobj, "sendermobile");
            request.SenderInfo.Phone = GetValue<string>(idict, dynamicobj, "senderphone");
            request.SenderInfo.Prov = GetValue<string>(idict, dynamicobj, "senderprovince");
            request.SenderInfo.Name = GetValue<string>(idict, dynamicobj, "sendername");
            request.ReceiverInfo.Address = GetValue<string>(idict, dynamicobj, "receiveraddress");
            request.ReceiverInfo.City = GetValue<string>(idict, dynamicobj, "receivercity");
            request.ReceiverInfo.County = GetValue<string>(idict, dynamicobj, "receivercountry");
            request.ReceiverInfo.Mobile = GetValue<string>(idict, dynamicobj, "receivermobile");
            request.ReceiverInfo.Phone = GetValue<string>(idict, dynamicobj, "receiverphone");
            request.ReceiverInfo.Prov = GetValue<string>(idict, dynamicobj, "receiverprovince");
            request.ReceiverInfo.Name = GetValue<string>(idict, dynamicobj, "receivername");
            if (this.LogisticsMethod != null)
            {
                request.LogisticsMethod = LogisticsMethod.Name;
            }

            request.OrderId = GetValue<Guid>(idict, dynamicobj, "orderId", "orderid");

            request.ReferenceId = GetValue<string>(idict, dynamicobj, "ref", "reference");

            return request;
        }

        private T GetValue<T>(System.Collections.IDictionary idict, IDictionary<string, object> Dynamic, params string[] fieldnames)
        {
            var type = typeof(T);

            object Value = null;

            foreach (var item in fieldnames)
            {
                if (idict != null)
                {
                    Value = Accessor.GetValueIDict(idict, item, type);
                }
                else if (Dynamic != null)
                {
                    Value = Accessor.GetValue(Dynamic, item, type);
                }

                if (Value != null)
                {
                    break;
                }
            }

            if (Value != null)
            {
                return (T)Value;
            }

            return default(T);
        }
    }
}

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
                Prov = GetValue<string>(idict, dynamicobj, "senderprovince"),
                City = GetValue<string>(idict, dynamicobj, "sendercity"),
                County = GetValue<string>(idict, dynamicobj, "sendercounty")
            };

            var receiverInfo = new Info
            {
                Prov = GetValue<string>(idict, dynamicobj, "receiverprovince"),
                City = GetValue<string>(idict, dynamicobj, "receivercity"),
                County = GetValue<string>(idict, dynamicobj, "receivercounty")
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

            var name = GetValue<string>(idict, dynamicobj, "cargoname");
            var cargoprice = GetValue<string>(idict, dynamicobj, "cargoprice");
            var cargoweight = GetValue<string>(idict, dynamicobj, "cargoweight");
            var cargocount = GetValue<string>(idict, dynamicobj, "cargocount");
            var currency = GetValue<string>(idict, dynamicobj, "cargocurrency");
            var unit = GetValue<string>(idict, dynamicobj, "cargounit");
            var sourcearea = GetValue<string>(idict, dynamicobj, "cargosourcearea");
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(cargoprice) || !string.IsNullOrEmpty(cargoweight)
                || !string.IsNullOrEmpty(cargocount) || !string.IsNullOrEmpty(currency) || !string.IsNullOrEmpty(unit)
                | !string.IsNullOrEmpty(sourcearea))
            {
                var cargo = new CargoInfo();
                cargo.Name = name;
                cargo.Price = cargoprice;
                cargo.Weight = cargoweight;
                cargo.Count = cargocount;
                cargo.Currency = currency;
                cargo.Unit = unit;
                cargo.SourceArea = sourcearea;
                request.CargoInfo = cargo;
            }

            var senderInfo = new Info();
            senderInfo.Country = GetValue<string>(idict, dynamicobj, "sendercountry");
            senderInfo.Address = GetValue<string>(idict, dynamicobj, "senderaddress");
            senderInfo.City = GetValue<string>(idict, dynamicobj, "sendercity");
            senderInfo.County = GetValue<string>(idict, dynamicobj, "sendercounty");
            senderInfo.Mobile = GetValue<string>(idict, dynamicobj, "sendermobile");
            senderInfo.Phone = GetValue<string>(idict, dynamicobj, "senderphone");
            senderInfo.Prov = GetValue<string>(idict, dynamicobj, "senderprovince");
            senderInfo.Name = GetValue<string>(idict, dynamicobj, "sendername");
            var receiverInfo = new Info();
            receiverInfo.Address = GetValue<string>(idict, dynamicobj, "receiveraddress");
            senderInfo.Country = GetValue<string>(idict, dynamicobj, "receivercountry");
            receiverInfo.City = GetValue<string>(idict, dynamicobj, "receivercity");
            receiverInfo.County = GetValue<string>(idict, dynamicobj, "receivercounty");
            receiverInfo.Mobile = GetValue<string>(idict, dynamicobj, "receivermobile");
            receiverInfo.Phone = GetValue<string>(idict, dynamicobj, "receiverphone");
            receiverInfo.Prov = GetValue<string>(idict, dynamicobj, "receiverprovince");
            receiverInfo.Name = GetValue<string>(idict, dynamicobj, "receivername");

            request.SenderInfo = senderInfo;
            request.ReceiverInfo = receiverInfo;

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

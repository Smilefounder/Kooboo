using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Logistics.Methods.yto.lib;
using Kooboo.Sites.Logistics.Methods.yto.Model;
using Kooboo.Sites.Logistics.Models;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.yto
{
    public class YTOLogistics : ILogisticsMethod<YTOSetting>
    {
        public YTOSetting Setting { get; set; }

        public string Name => "YTOLogistics";

        public string DisplayName => Data.Language.Hardcoded.GetValue("YTO", Context);

        public RenderContext Context { get; set; }

        [Description(@"
        <script engine='kscript'>
        var request = {};
        request.senderaddress='软件园三期',
        request.sendercity='厦门市',
        request.sendercounty='集美区',
        request.senderprovince='福建省',
        request.sendername='sender',
        request.senderphone='111111',
        request.receiveraddress='后龙镇',
        request.receivercity='泉州市',
        request.receivercounty='泉港区',
        request.receiverprovince='福建省',
        request.receivername='receive',
        request.receiverphone='11111111',
        k.logistics.yTOLogistics.createOrder(request)
</script>")]
        [KDefineType(Return = typeof(LogisticsResponse))]
        public ILogisticsResponse CreateOrder(LogisticsRequest request)
        {
            var createRequest = GenerateCreateOderRequest(request);
            LogisticsResponse res = null;

            if (Setting == null)
                return res;

            var apiClient = new YTOClient(this.Setting);
            var result = apiClient.CreateOrder(createRequest);
            if (result.Success)
            {
                res = new LogisticsResponse();
                res.requestId = request.Id;
                res.logisticsMethodReferenceId = result.TxLogisticID;
            }

            return res;
        }

        public LogisticsCallback Notify(RenderContext context)
        {
            LogisticsCallback callback = null;
            var logisticsMsg = ConvertOrderTrace(context.Request.Body);
            var model = XmlSerializerUtilis.DeserializeXML<OrderTracePushRequest>(logisticsMsg);
            if (model != null)
            {
                Guid logisticsRequestId;
                if (Guid.TryParse(model.TxLogisticID, out logisticsRequestId))
                {
                    callback = new LogisticsCallback()
                    {
                        RequestId = logisticsRequestId,
                        StatusMessage = model.Remark
                    };
                }
            }
            return callback;
        }

        public LogisticsStatusResponse checkStatus(LogisticsRequest request)
        {
            var traceRequest = new OrderTraceRequest()
            {
                Number = request.ReferenceId
            };

            var apiClient = new YTOClient(this.Setting);
            var result = apiClient.TraceOrder(traceRequest);
            if (result == null)
            {
                return null;
            }

            return new LogisticsStatusResponse
            {
                RequestId = request.Id,
                StatusMessage = result.ProcessInfo,
                BillCode = request.ReferenceId
            };
        }

        [Description(@"
        <script engine='kscript'>
         var request = {};
        request.senderprovince='江苏省',
        request.sendercity='厦门市',
        request.receiverprovince='福建省',
        request.receivercity='泉州市',
        request.cargoweight='1',
        k.logistics.YTOLogistics.getPostage(request)
        </script> ")]
        public string GetPostage(LogisticsRequest request)
        {
            if (Setting == null)
            {
                return "";
            }
            var chargeRequest = new ChargeQueryRequest
            {
                StartProvince = request.SenderInfo.Prov,
                StartCity = request.SenderInfo.City,
                EndProvince = request.ReceiverInfo.Prov,
                EndCity = request.ReceiverInfo.City,
                GoodsWeight = request.Weight.ToString()
            };

            var apiClient = new YTOClient(this.Setting);
            var result = apiClient.ChargeQuery(chargeRequest);
            return result;
        }

        private RequestOrder GenerateCreateOderRequest(LogisticsRequest request)
        {
            var requestOrder = new RequestOrder
            {
                Receiver = new PersonalInfo
                {
                    Address = "软件园",
                    City = "厦门市",
                    Prov = "福建省",
                    Name = "receiver",
                    Phone = "11111111111",
                    PostCode = "0"
                },
                Sender = new PersonalInfo
                {
                    Address = "软件园",
                    City = "泉州市",
                    Prov = "福建省",
                    Name = "receiver",
                    Phone = "11111111111",
                    PostCode = "0"
                },
                TxLogisticID = request.Id.ToString("N")
            };
            return requestOrder;
        }

        private string ConvertOrderTrace(string body)
        {
            var param = body.Split('&');
            foreach (var item in param)
            {
                if (item.Contains("logistics_interface"))
                {
                    var request = item.Split('=');
                    if (!string.IsNullOrEmpty(request[1]))
                    {
                        return HttpUtility.UrlDecode(request[1], Encoding.UTF8);
                    }
                }
            }

            return null;
        }
    }
}

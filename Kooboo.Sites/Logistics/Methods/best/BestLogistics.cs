using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Logistics.Methods.best;
using Kooboo.Sites.Logistics.Methods.best.lib;
using Kooboo.Sites.Logistics.Methods.best.Model;
using Kooboo.Sites.Logistics.Methods.zop;
using Kooboo.Sites.Logistics.Methods.zop.lib;
using Kooboo.Sites.Logistics.Methods.zop.Models;
using Kooboo.Sites.Logistics.Models;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.zop
{
    public class BestLogistics : ILogisticsMethod<BestSetting>
    {
        public BestSetting Setting { get; set; }

        public string Name => "BestLogistics";

        public string DisplayName => Data.Language.Hardcoded.GetValue("Best", Context);

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
        k.logistics.bestLogistics.createOrder(request)
</script>")]
        [KDefineType(Return = typeof(LogisticsResponse))]
        public ILogisticsResponse CreateOrder(LogisticsRequest request)
        {
            request.ReferenceId = "TEST000000051";
            checkStatus(request);
            LogisticsResponse res = null;

            if (Setting == null)
                return res;

            var oderRequest = GenerateCreateOderRequest(request);
            var apiClient = new BestClient(this.Setting);
            var result = apiClient.CreateOrder(oderRequest);
            res = new LogisticsResponse();
            request.ReferenceId = result.remark;
            res.requestId = request.Id;
            res.logisticsMethodReferenceId = result.remark;

            return res;
        }

        public LogisticsStatusResponse checkStatus(LogisticsRequest request)
        {
            var apiClient = new BestClient(this.Setting);
            var result = apiClient.TraceOrder(request.ReferenceId);
            if (result == null)
            {
                return null;
            }

            var trace = result.traceLogs.FirstOrDefault()?.traces?.trace.FirstOrDefault()?.scanType ?? "";
            var status = result.traceLogs.FirstOrDefault()?.problems.problem.Count > 0 ? OrderStatus.Problem : ConvertStatus(trace);

            return new LogisticsStatusResponse
            {
                RequestId = request.Id,
                Status = status,
                BillCode = request.ReferenceId
            };
        }

        public string GetPostage(LogisticsRequest request)
        {
            return "";
        }

        private BestCreateOrderRequest GenerateCreateOderRequest(LogisticsRequest request)
        {
            var sender = new Sender
            {
                name = request.SenderInfo.Name,
                address = request.SenderInfo.Address,
                city = request.SenderInfo.City,
                county = request.SenderInfo.County,
                phone = request.SenderInfo.Phone,
                prov = request.SenderInfo.Prov
            };

            var receiver = new Receiver
            {
                name = request.ReceiverInfo.Name,
                address = request.ReceiverInfo.Address,
                city = request.ReceiverInfo.City,
                county = request.ReceiverInfo.County,
                phone = request.ReceiverInfo.Phone,
                prov = request.ReceiverInfo.Prov
            };

            var orderRequest = new BestCreateOrderRequest
            {
                sender = sender,
                receiver = receiver,
                txLogisticID = request.Id.ToString("N")
            };

            return orderRequest;
        }

        private OrderStatus ConvertStatus(string code)
        {
            var status = OrderStatus.Init;
            switch (code.ToUpper())
            {
                case "收件":
                    status = OrderStatus.Got;
                    break;
                case "发件":
                    status = OrderStatus.Got;
                    break;
                case "派件":
                    status = OrderStatus.Scan;
                    break;
                case "签收":
                    status = OrderStatus.Signed;
                    break;
                case "代理点签收":
                    status = OrderStatus.ThirdPartSign;
                    break;
                case "用户提货":
                    status = OrderStatus.Signed;
                    break;
            }

            return status;
        }
    }
}

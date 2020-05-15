using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Logistics.Methods.zop;
using Kooboo.Sites.Logistics.Methods.zop.lib;
using Kooboo.Sites.Logistics.Methods.zop.Models;
using Kooboo.Sites.Logistics.Models;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.zop
{
    public class ZOPLogistics : ILogisticsMethod<ZOPSetting>
    {
        public ZOPSetting Setting { get; set; }

        public string Name => "ZTOLogistics";

        public string DisplayName => Data.Language.Hardcoded.GetValue("ZTO", Context);

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
        k.logistics.zTOLogistics.createOrder(request)
</script>")]
        [KDefineType(Return = typeof(LogisticsResponse))]
        public ILogisticsResponse CreateOrder(LogisticsRequest request)
        {
            LogisticsResponse res = null;

            if (Setting == null)
                return res;

            var ztoModel = GenerateCreateOderRequest(request);
            var apiClient = new ZOPClient(this.Setting);
            
            var result = apiClient.CreateOrder(ztoModel);
            if (!string.IsNullOrEmpty(result))
            {
                request.ReferenceId = result;
                res.requestId = request.Id;
                res.logisticsMethodReferenceId = result;
                TraceSubscribe(apiClient, result);
            }

            return res;
        }

        public LogisticsStatusResponse checkStatus(LogisticsRequest request)
        {
            var apiClient = new ZOPClient(this.Setting);
            var result = apiClient.TraceOrder(request.ReferenceId);
            if (result == null)
            {
                return null;
            }

            var status = result.Traces.First() == null ? OrderStatus.Init : ConvertStatus(result.Traces.First().ScanType);

            return new LogisticsStatusResponse
            {
                RequestId = request.Id,
                Status = status,
                BillCode = request.ReferenceId
            };
        }

        [Description(@"
        <script engine='kscript'>
         var request = {};
        request.senderprovince='江苏省',
        request.receiverprovince='福建省',
        request.cargoweight='1',
        k.logistics.zTOLogistics.getPostage(request)
        </script> ")]
        public string GetPostage(LogisticsRequest request)
        {
            if (Setting == null)
            {
                return "";
            }

            var ztoModel = new ZOPRequest();
            ztoModel.addParam("companyCode", Setting.CompanyCode);
            ztoModel.addParam("provinceStart", request.ReceiverInfo.Prov);
            ztoModel.addParam("provinceEnd", request.ReceiverInfo.Prov);
            ztoModel.addParam("weight", request.Weight.ToString("0.00"));

            var apiClient = new ZOPClient(this.Setting);
            var result = apiClient.GetPostage(ztoModel);
            return result;
        }

        public LogisticsCallback Notify(RenderContext context)
        {
            LogisticsCallback callback = null;
            var data = GetOrderTrace(context.Request.Body);
            var response = JsonConvert.DeserializeObject<OrderTracePushRequest>(data);
            if(response!=null)
            {
                var request = LogisticsManager.GetRequestByReferece(response.BillCode, context);

                if (request != null)
                {

                    callback = new LogisticsCallback()
                    {
                        RequestId = request.Id,
                        StatusMessage = response.Desc
                    };
                }
            }

            return callback;
        }

        private void TraceSubscribe(ZOPClient client,string billCode)
        {
            //https://zop.zto.com/apiDoc/
            var ztoModel = new ZOPRequest();
            ztoModel.addParam("billCode", billCode);
            ztoModel.addParam("action", Setting.Actions);
            ztoModel.addParam("problemCode", Setting.ProblemCodes);
            ztoModel.addParam("pushUrl", LogisticsHelper.GetCallbackUrl(this, nameof(Notify), Context));
            ztoModel.addParam("token", Guid.NewGuid().ToString());

            var result = client.TraceSubscribe(ztoModel);
            if(!result)
            {
                throw new Exception("订阅轨迹失败！");
            }
        }

        private string GetOrderTrace(string body)
        {
            var param = body.Split('&');
            foreach (var item in param)
            {
                if (item.Contains("data"))
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

        private ZOPRequest GenerateCreateOderRequest(LogisticsRequest request)
        {
            var senderInfo = new Dictionary<string, string>();
            senderInfo.Add("address", request.SenderInfo.Address);
            senderInfo.Add("city", request.SenderInfo.City);
            senderInfo.Add("county", request.SenderInfo.County);
            senderInfo.Add("phone", request.SenderInfo.Phone);
            senderInfo.Add("prov", request.SenderInfo.Prov);
            senderInfo.Add("mobile", request.SenderInfo.Mobile);
            senderInfo.Add("name", request.SenderInfo.Name);
            var receiverInfo = new Dictionary<string, string>();
            receiverInfo.Add("address", request.ReceiverInfo.Address);
            receiverInfo.Add("city", request.ReceiverInfo.City);
            receiverInfo.Add("county", request.ReceiverInfo.County);
            receiverInfo.Add("phone", request.ReceiverInfo.Phone);
            receiverInfo.Add("prov", request.ReceiverInfo.Prov);
            receiverInfo.Add("mobile", request.ReceiverInfo.Mobile);
            receiverInfo.Add("name", request.ReceiverInfo.Name);

            var orderInfo = new Dictionary<string, string>();
            orderInfo.Add("companyCode", Setting.CompanyCode);
            orderInfo.Add("partnerCode", request.Id.ToString("N"));
            orderInfo.Add("hallCode", Setting.HallCode);
            orderInfo.Add("sender", JsonConvert.SerializeObject(senderInfo));
            orderInfo.Add("receiver", JsonConvert.SerializeObject(receiverInfo));

            var systemParameter = new Dictionary<string, string>();
            systemParameter.Add("serviceCode", Setting.ServiceCode);

            var ztoModel = new ZOPRequest();
            ztoModel.addParam("systemParameter", JsonConvert.SerializeObject(systemParameter));
            ztoModel.addParam("orderInfo", JsonConvert.SerializeObject(orderInfo));

            return ztoModel;
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
                case "第三方签收":
                    status = OrderStatus.ThirdPartSign;
                    break;
                case "ARRIVAL":
                    status = OrderStatus.ThirdPartSign;
                    break;
                case "SIGNED":
                    status = OrderStatus.Signed;
                    break;
                case "退件":
                    status = OrderStatus.Failed;
                    break;
                case "退件签收":
                    status = OrderStatus.FailedSigned;
                    break;
                case "问题件":
                    status = OrderStatus.Problem;
                    break;
            }

            return status;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Logistics.Methods.deppon.lib;
using Kooboo.Sites.Logistics.Methods.deppon.Model;
using Kooboo.Sites.Logistics.Models;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.deppon
{
    public class DEPPONLogistics : ILogisticsMethod<DEPPONSetting>
    {
        public DEPPONSetting Setting { get; set; }

        public string Name => "DEPPONLogistics";

        public string DisplayName => Data.Language.Hardcoded.GetValue("DEPPON", Context);

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
request.cargoname='test',
request.cargocount='2',
request.cargoweight='1'
        k.logistics.dEPPONLogistics.createOrder(request)
</script>")]
        [KDefineType(Return = typeof(LogisticsResponse))]
        public ILogisticsResponse CreateOrder(LogisticsRequest request)
        {
            LogisticsResponse res = null;
            if (Setting == null)
                return res;

            request.Additional.TryGetValue("cargoname", out var cargoName);
            request.Additional.TryGetValue("cargocount", out var cargoCount);
            request.Additional.TryGetValue("cargoweight", out var cargoWeight);

            if (cargoName == null || cargoCount == null || cargoWeight == null)
            {
                return res;
            }
            var orderRequest = GenerateCreateOderRequest(request);
            var packageInfo = new PackageInfo
            {
                CargoName = request.CargoInfo.Name,
                TotalNumber = request.CargoInfo.Count,
                TotalWeight = request.CargoInfo.Weight,
                DeliveryType = "3",//1、自提； 2、送货进仓； 3、送货（不含上楼）； 4、送货上楼； 5、大件上楼
            };
            orderRequest.PackageInfo = packageInfo;

            var apiClient = new DEPPONClient(this.Setting);
            var result = apiClient.CreateOrder(orderRequest);
            if (result != null)
            {
                res = new LogisticsResponse();
                request.ReferenceId = result.MailNo;
                res.requestId = request.Id;
                res.logisticsMethodReferenceId = result.MailNo;
                //状态订阅
                apiClient.TraceSubscribe(result.MailNo);
            }

            return res;
        }

        public LogisticsStatusResponse checkStatus(LogisticsRequest request)
        {
            LogisticsStatusResponse res = null;

            var dic = new Dictionary<string, string>();
            dic.Add("mailNo", request.ReferenceId);
            var apiClient = new DEPPONClient(this.Setting);
            var result = apiClient.TraceOrder(dic);

            if (result != null)
            {
                res = new LogisticsStatusResponse
                {
                    RequestId = request.Id,
                    BillCode = request.ReferenceId,
                    Status = ConvertStatus(result.Status),
                    StatusMessage = result.Description
                };
            }

            return res;

        }

        public LogisticsCallback Notify(RenderContext context)
        {
            LogisticsCallback callback = null;
            var response = JsonConvert.DeserializeObject<TraceSubscribeReponse>(context.Request.Body);
            if (response == null)
            {
                return null;
            }

            var track = response.TrackList?.FirstOrDefault();
            var trace = track?.TraceList?.FirstOrDefault();
            if (trace != null)
            {
                var request = LogisticsManager.GetRequestByReferece(track.Number, context);

                if (request != null)
                {

                    callback = new LogisticsCallback()
                    {
                        RequestId = request.Id,
                        Status = ConvertStatus(trace.Status),
                        StatusMessage = trace.Description
                    };
                }
            }

            return callback;
        }

        [Description(@"
        <script engine='kscript'>
         var request = {};
        request.senderprovince='北京省',
        request.sendercity='北京市',
        request.sendercounty='大兴区',
        request.receiverprovince='福建省',
        request.receivercity='泉州市',
        request.receivercounty='泉港区',
        request.cargoweight='1',
        k.logistics.dEPPONLogistics.getPostage(request)
        </script>")]
        public string GetPostage(LogisticsRequest request)
        {
            if (Setting == null)
            {
                return "";
            }

            var postageRequest = new PostageRequest();
            string receiverAddress = string.Format("{0}-{1}-{2}", request.ReceiverInfo.Prov, request.ReceiverInfo.City, request.ReceiverInfo.County);
            string senderAddress = string.Format("{0}-{1}-{2}", request.SenderInfo.Prov, request.SenderInfo.City, request.SenderInfo.County);
            postageRequest.Originalsaddress = receiverAddress;
            postageRequest.OriginalsStreet = senderAddress;
            postageRequest.TotalWeight = request.Weight.ToString();

            var apiClient = new DEPPONClient(this.Setting);
            var result = apiClient.GetPostage(postageRequest);
            if (result == null)
            {
                return "查询新官网";
            }

            return result;
        }

        private CreateOrderRequest GenerateCreateOderRequest(LogisticsRequest request)
        {
            var orderRequest = new CreateOrderRequest();

            var sender = new Sender
            {
                Name = request.SenderInfo.Name,
                Mobile = request.SenderInfo.Phone,
                Province = request.SenderInfo.Prov,
                City = request.SenderInfo.City,
                County = request.SenderInfo.County,
                Address = request.SenderInfo.Address,
            };

            var receiver = new Receiver
            {
                Name = request.ReceiverInfo.Name,
                Mobile = request.ReceiverInfo.Phone,
                Province = request.ReceiverInfo.Prov,
                City = request.ReceiverInfo.City,
                County = request.ReceiverInfo.County,
                Address = request.ReceiverInfo.Address,
            };

            orderRequest.GmtCommit = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss");
            orderRequest.PayType = "2";//0:发货人付款（现付） 1:收货人付款（到付） 2：发货人付款（月结） （电子运单客户不支持寄付）
            orderRequest.Sender = sender;
            orderRequest.Receiver = receiver;
            orderRequest.LogisticID = Setting.Sign + request.Id.ToString();
            return orderRequest;
        }

        private OrderStatus ConvertStatus(string code)
        {
            var status = OrderStatus.Init;
            switch (code.ToUpper())
            {
                case "GOT":
                    status = OrderStatus.Got;
                    break;
                case "ARRIVAL":
                    status = OrderStatus.Arrival;
                    break;
                case "SENT_SCAN":
                    status = OrderStatus.Scan;
                    break;
                case "SIGNED":
                    status = OrderStatus.Signed;
                    break;
                case "ERROR":
                    status = OrderStatus.Problem;
                    break;
                case "FAILED":
                    status = OrderStatus.Failed;
                    break;
            }

            return status;
        }
    }
}
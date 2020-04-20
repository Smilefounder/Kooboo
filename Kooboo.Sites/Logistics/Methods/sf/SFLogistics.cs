using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Logistics.Methods.sf.lib;
using Kooboo.Sites.Logistics.Methods.sf.Model;
using Kooboo.Sites.Logistics.Models;

namespace Kooboo.Sites.Logistics.Methods.sf
{
    class SFLogistics : ILogisticsMethod<SFSetting>
    {
        public SFSetting Setting { get; set; }

        public string Name => "SFLogistics";

        public string DisplayName => Data.Language.Hardcoded.GetValue("SF", Context);

        public RenderContext Context { get; set; }

        [Description(@"
        <script engine='kscript'>
        var request = {};
 request.receiver_company='test',
    request.senderaddress='软件园三期',
request.sendercity='厦门市',
request.sendercounty='集美区',
request.senderprovince='福建省',
request.sendername='sender',
request.senderphone='111111',
request.receiveraddress='后龙镇',
request.receivercity='泉州市',
request.receivercountry='泉港区',
request.receiverprovince='福建省',
request.receivername='receive',
request.receiverphone='11111111',
        k.logistics.sFLogistics.createOrder(request)
</script>")]
        [KDefineType(Return = typeof(LogisticsResponse))]
        public ILogisticsResponse CreateOrder(LogisticsRequest request)
        {
            request.ReferenceId = "444003077898";
            checkStatus(request);
            LogisticsResponse res = null;

            if (Setting == null)
                return res;

            var sfModel = GenerateCreateOderRequest(request);
            var apiClient = new SFClient(this.Setting);
            var result = apiClient.CreateOrder(sfModel);
            if (result != null)
            {
                request.ReferenceId = result.Body.MailNo;
                res.requestId = request.Id;
                res.logisticsMethodReferenceId = result.Body.MailNo;
            }

            return res;
        }

        public LogisticsStatusResponse checkStatus(LogisticsRequest request)
        {
            LogisticsStatusResponse res = null;
            var traceRequest = new TraceOrderRequest
            {
                TrackingNumber = request.ReferenceId
            };
            var apiClient = new SFClient(this.Setting);
            var result = apiClient.TraceOrder(traceRequest);

            if(result !=null)
            {
                res = new LogisticsStatusResponse
                {
                    RequestId = request.Id,
                    BillCode = request.ReferenceId,
                    Status = ConvertStatus(result.Code),
                    StatusMessage = result.Remark
                };
            }

            return res;
        }

        public string GetPostage(LogisticsRequest request)
        {
            //顺丰不提供这个借口 是通过月结算
            return "";
        }

        private CreateOrderRequest GenerateCreateOderRequest(LogisticsRequest request)
        {
            request.Additional.TryGetValue("delivery_code", out var deliverycode);
            request.Additional.TryGetValue("receiver_company", out var receivercompany);
            request.Additional.TryGetValue("receiver_postcode", out var receiverpostcode);

            request.Additional.TryGetValue("sender_company", out var sendercompany);
            request.Additional.TryGetValue("sender_postcode", out var senderpostcode);

            request.Additional.TryGetValue("declared_value", out var declareValue);
            request.Additional.TryGetValue("j_shippercode", out var shippercode);

            var orderRequest = new CreateOrderRequest();

            var orderInfo = new OrderInfo
            {
                DAddress = request.ReceiverInfo.Address,
                DTel = request.ReceiverInfo.Phone,
                DCountry = request.ReceiverInfo.Country,
                DCounty = request.ReceiverInfo.County,
                DCity = request.ReceiverInfo.City,
                DContact = request.ReceiverInfo.Name,
                DProvince = request.ReceiverInfo.Prov,
                DDeliverycode = deliverycode?.ToString(),
                DCompany = receivercompany?.ToString(),
                DPostCode = receiverpostcode?.ToString(),
                JAddress = request.ReceiverInfo.Address,
                JTel = request.ReceiverInfo.Phone,
                JCountry = request.ReceiverInfo.Country,
                JCounty = request.ReceiverInfo.County,
                JCity = request.ReceiverInfo.City,
                JContact = request.ReceiverInfo.Name,
                JProvince = request.ReceiverInfo.Prov,
                JCompany = sendercompany?.ToString(),
                JPostCode = senderpostcode?.ToString(),
                DeclaredValue = declareValue?.ToString(),
                JShippercode = shippercode?.ToString(),
                OrderID = request.Id.ToString("N"),
                CustId = Setting.CustId
            };

            if (request.CargoInfo != null)
            {
                var cargo = new Cargo
                {
                    Name = request.CargoInfo.Name,
                    Unit = request.CargoInfo.Unit,
                    Currency = request.CargoInfo.Currency,
                    Weight = request.CargoInfo.Weight,
                    Amount = request.CargoInfo.Price,
                    SourceArea = request.CargoInfo.SourceArea,
                    Count = request.CargoInfo.Count
                };
                orderRequest.OrderInfo.Cargo = cargo;
            }

            orderRequest.OrderInfo = orderInfo;
            return orderRequest;
        }

        private OrderStatus ConvertStatus(string code)
        {
            var status = OrderStatus.Init;
            switch (code.ToUpper())
            {
                case "30":
                    status = OrderStatus.Got;
                    break;
                case "31":
                    status = OrderStatus.Got;
                    break;
                case "44":
                    status = OrderStatus.Scan;
                    break;
                case "607":
                    status = OrderStatus.ThirdPartSign;
                    break;
                case "80":
                    status = OrderStatus.Signed;
                    break;
                case "648":
                    status = OrderStatus.Failed;
                    break;
                case "33":
                    status = OrderStatus.Problem;
                    break;
            }

            return status;
        }
    }
}

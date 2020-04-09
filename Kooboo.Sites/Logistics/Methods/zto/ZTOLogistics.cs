using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Logistics.Methods.zto;
using Kooboo.Sites.Logistics.Methods.zto.lib;
using Kooboo.Sites.Logistics.Methods.zto.Models;
using Kooboo.Sites.Logistics.Models;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.zto
{
    public class ZTOLogistics : ILogisticsMethod<ZTOSetting>
    {
        public ZTOSetting Setting { get; set; }

        public string Name => "ZTOLogistics";

        public string DisplayName => Data.Language.Hardcoded.GetValue("ZTO", Context);

        public RenderContext Context { get; set; }

        [Description(@"
        <script engine='kscript'>
        var request = {};
request.senderaddress='软件园三期',
request.sendercity='厦门市',
request.sendercountry='集美区',
request.senderprovince='福建省',
request.sendername='sender',
request.senderphone='111111',
request.senderaddress='后龙镇',
request.sendercity='泉州市',
request.sendercountry='泉港区',
request.senderprovince='福建省',
request.receivername='receive',
request.receiverphone='11111111'
        </script> ")]
        [KDefineType(Return = typeof(LogisticsResponse))]
        public ILogisticsResponse CreateOrder(LogisticsRequest request)
        {
            LogisticsResponse res = null;

            if (Setting == null)
                return res;

            var ztoModel = GenerateCreateOderRequest(request);
            var apiClient = new ZopClient(this.Setting);
            var result = apiClient.CreateOrder(ztoModel);
            if (!string.IsNullOrEmpty(result))
            {
                request.ReferenceId = result;
                res.requestId = request.Id;
                res.logisticsMethodReferenceId = result;
            }
                
            return res;
        }

        public LogisticsStatusResponse checkStatus(LogisticsRequest request)
        {
            return new LogisticsStatusResponse();
        }

        [Description(@"
        <script engine='kscript'>
        var request = {};
        request.senderprovince='江苏省',
        request.receiverprovince='福建省',
        request.cargoweight='1'
        </script> ")]
        public string GetPostage(LogisticsRequest request)
        {
            if (Setting == null)
            {
                return "";
            }

            var ztoModel = new ZopRequest();
            ztoModel.addParam("companyCode", Setting.CompanyCode);
            ztoModel.addParam("provinceStart", request.ReceiverInfo.Prov);
            ztoModel.addParam("provinceEnd", request.ReceiverInfo.Prov);
            ztoModel.addParam("weight", request.Weight.ToString("0.00"));

            var apiClient = new ZopClient(this.Setting);
            var result = apiClient.GetPostage(ztoModel);
            return result;
        }

        private ZopRequest GenerateCreateOderRequest(LogisticsRequest request)
        {
            var senderInfo = new Dictionary<string, string>();
            senderInfo.Add("address", request.SenderInfo.Address);
            senderInfo.Add("city", request.SenderInfo.City);
            senderInfo.Add("county", request.SenderInfo.County);
            senderInfo.Add("phone", request.SenderInfo.Phone);
            senderInfo.Add("prov", request.SenderInfo.Prov);
            senderInfo.Add("mobile", request.SenderInfo.Mobile);
            senderInfo.Add("mobile", request.SenderInfo.Name);
            var receiverInfo = new Dictionary<string, string>();
            receiverInfo.Add("address", request.ReceiverInfo.Address);
            receiverInfo.Add("city", request.ReceiverInfo.City);
            receiverInfo.Add("county", request.ReceiverInfo.County);
            receiverInfo.Add("phone", request.ReceiverInfo.Phone);
            receiverInfo.Add("prov", request.ReceiverInfo.Prov);
            receiverInfo.Add("mobile", request.ReceiverInfo.Mobile);
            receiverInfo.Add("mobile", request.ReceiverInfo.Name);

            var orderInfo = new Dictionary<string, string>();
            orderInfo.Add("companyCode", Setting.CompanyCode);
            orderInfo.Add("partnerCode", request.Id.ToString());
            orderInfo.Add("hallCode", Setting.HallCode);
            orderInfo.Add("sender", JsonConvert.SerializeObject(senderInfo));
            orderInfo.Add("receiver", JsonConvert.SerializeObject(receiverInfo));

            var systemParameter = new Dictionary<string, string>();
            systemParameter.Add("serviceCode", Setting.ServiceCode);

            var ztoModel = new ZopRequest();
            ztoModel.addParam("systemParameter", JsonConvert.SerializeObject(systemParameter));
            ztoModel.addParam("orderInfo", JsonConvert.SerializeObject(orderInfo));

            return ztoModel;
        }
    }
}

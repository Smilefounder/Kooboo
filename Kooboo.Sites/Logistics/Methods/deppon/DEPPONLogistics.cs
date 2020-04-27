using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        k.logistics.dEPPONLogistics.createOrder(request)
</script>")]
        [KDefineType(Return = typeof(LogisticsResponse))]
        public ILogisticsResponse CreateOrder(LogisticsRequest request)
        {
            LogisticsResponse res = null;

            if (Setting == null)
                return res;
            return res;
        }

        public LogisticsStatusResponse checkStatus(LogisticsRequest request)
        {
            return new LogisticsStatusResponse(); 
        }

        [Description(@"
        <script engine='kscript'>
         var request = {};
        request.senderprovince='北京省',
        request.sendercity='北京市',
        request.sendercounty='大兴区,
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
            postageRequest.DestProvince = request.ReceiverInfo.Prov;
            postageRequest.DestDistrict = request.ReceiverInfo.County;
            postageRequest.DestCity = request.ReceiverInfo.City;
            postageRequest.OriginalProvince = request.SenderInfo.Prov;
            postageRequest.OriginalCity = request.SenderInfo.City;

            var apiClient = new DEPPONClient(this.Setting);
            var result = apiClient.GetPostage(postageRequest);
            return result;
        }
    }
}
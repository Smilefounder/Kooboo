using Kooboo.Data.Context;
using Kooboo.Sites.Logistics.Methods.sto.lib;
using Kooboo.Sites.Logistics.Methods.sto.Models;
using Kooboo.Sites.Logistics.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Kooboo.Sites.Logistics.Methods.sto
{
    public class STOLogistics : ILogisticsMethod<STOSetting>
    {
        public STOSetting Setting { get; set; }

        public string Name => "STOLogistics";

        public string DisplayName => Data.Language.Hardcoded.GetValue("STO", Context);

        public RenderContext Context { get; set; }

        public LogisticsStatusResponse checkStatus(LogisticsRequest request)
        {
            var apiClient = new STOClient(this.Setting);
            var waybillNoList = new List<string> { request.ReferenceId, "6606002282568" };

            var apiResult = apiClient.TraceOrder(waybillNoList);

            // 申通 api返回数据跟文档不符，需要确认，暂时电话联系不上
            return new LogisticsStatusResponse
            {

            };
        }

        [Description(@"<script engine='kscript'>
        var request = {};
        request.sendername='小李';
        request.senderphone='18080808080';
        request.senderprovince='福建省';
        request.sendercity='厦门市';
        request.sendercounty='集美区';
        request.senderaddress='软件园三期';

        request.receivername='小明';
        request.receiverphone='19090909090';
        request.receiverprovince='福建省';
        request.receivercity='泉州市';
        request.receivercounty='丰泽区';
        request.receiveraddress='xxx地址';

        request.cargoname='xx服装';
        request.cargocount='1';
        // kg
        request.cargoweight='2';

        request.remark='无备注';

        k.logistics.sTOLogistics.createOrder(request);
</script>")]
        public ILogisticsResponse CreateOrder(LogisticsRequest request)
        {
            LogisticsResponse res = null;
            if (Setting == null)
                return res;

            var apiClient = new STOClient(this.Setting);
            var apiResult = apiClient.CreateOrder(request);

            request.ReferenceId = apiResult.data.waybillNo;
            res = new LogisticsResponse();
            res.requestId = request.Id;
            res.logisticsMethodReferenceId = apiResult.data.waybillNo;
            return res;
        }

        public string GetPostage(LogisticsRequest request)
        {
            return "";
        }

        public LogisticsCallback Notify(RenderContext context)
        {
            LogisticsCallback callback = null;

            // https://open.sto.cn/#/apiDocument/STO_TRACE_PLATFORM_PUSH
            var notifyContentStr = context.Request.GetValue("content");
            if (notifyContentStr == null)
            {
                return callback;
            }

            var response = JsonConvert.DeserializeObject<STONotifyContent>(notifyContentStr);
            if (response == null || string.IsNullOrEmpty(response.linkCode))
            {
                return callback;
            }

            Guid logisticsRequestId;
            if (Guid.TryParse(response.linkCode, out logisticsRequestId))
            {
                callback = new LogisticsCallback()
                {
                    RequestId = logisticsRequestId,
                    Status = ConvertStatus(response.trace.scanType),
                    StatusMessage = response.trace.memo
                };
            }

            return callback;
        }

        private OrderStatus ConvertStatus(string scanType)
        {
            var status = OrderStatus.Init;
            switch (scanType.ToUpper())
            {
                case "收件":
                    status = OrderStatus.Got;
                    break;
                case "发件":
                    status = OrderStatus.Got;
                    break;
                case "派件":
                case "到件":
                    status = OrderStatus.Scan;
                    break;
                case "签收":
                    status = OrderStatus.Signed;
                    break;
                case "代理点签收":
                    status = OrderStatus.ThirdPartSign;
                    break;
            }

            return status;
        }
    }
}

using Kooboo.Sites.Logistics.Methods.sto.Models;
using Kooboo.Sites.Logistics.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Sites.Logistics.Methods.sto.lib
{
    public class STOClient
    {
        private STOSetting setting;
        private const string CreateOrderAPIName = "OMS_EXPRESS_ORDER_CREATE";
        private const string CreateOrderRegistApplyKey = "sto_oms";
        private const string TraceOrderAPIName = "STO_TRACE_QUERY_COMMON";
        private const string TraceOrderRegistApplyKey = "sto_trace_query";

        public STOClient(STOSetting setting)
        {
            this.setting = setting;
        }

        public STOCreateOrderResponse CreateOrder(LogisticsRequest request)
        {
            // https://open.sto.cn/#/apiDocument/OMS_EXPRESS_ORDER_CREATE
            var content = GenerateCreateOrderContent(request);

            string postData = GeneratePostData(CreateOrderAPIName, JsonConvert.SerializeObject(content), CreateOrderRegistApplyKey);
            var apiResponse = DoPost(setting.ServerURL, postData).Result;

            var response = JsonConvert.DeserializeObject<STOCreateOrderResponse>(apiResponse.Content);
            if (response.success && response.data != null)
            {
                return response;
            }
            else
            {
                throw new Exception($"Error Code: {response.errorCode}; content: {response.errorMsg}.");
            }
        }

        public STOTraceOrderResponse TraceOrder(List<string> waybillNoList)
        {
            // https://open.sto.cn/#/apiDocument/STO_TRACE_QUERY_COMMON
            var content = new STOTraceOrderContent
            {
                order = "asc",
                waybillNoList = waybillNoList
            };

            string postData = GeneratePostData(TraceOrderAPIName, JsonConvert.SerializeObject(content), TraceOrderRegistApplyKey);
            var apiResponse = DoPost(setting.ServerURL, postData).Result;

            var response = JsonConvert.DeserializeObject<STOTraceOrderResponse>(apiResponse.Content);
            if (response.success && response.data != null)
            {
                return response;
            }
            else
            {
                throw new Exception($"Error Code: {response.errorCode}; content: {response.errorMsg}.");
            }
        }

        public static async Task<Lib.Helper.ApiClient.ApiResponse> DoPost(string queryUrl, string postData)
        {
            var client = Lib.Helper.ApiClient.Create();
            var result = await client.PostAsync(queryUrl, postData, "application/x-www-form-urlencoded");
            return result;
        }

        public static string CalculateDigest(string content, string secretKey)
        {
            string toSignContent = content + secretKey;
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.GetEncoding("utf-8").GetBytes(toSignContent);
            byte[] hash = md5.ComputeHash(inputBytes);
            return Convert.ToBase64String(hash);
        }

        public int ParseInt(string strValue)
        {
            int result;
            if (int.TryParse(strValue, out result))
            {
                return result;
            }

            return result;
        }

        private STOCreateOrderContent GenerateCreateOrderContent(LogisticsRequest request)
        {
            request.Additional.TryGetValue("remark", out var remark);

            var content = new STOCreateOrderContent
            {
                orderNo = request.Id.ToString("N"),
                orderSource = "VIPEO",
                billType = "00",
                orderType = "01",
                sender = request.SenderInfo == null ? new sender() : new sender
                {
                    name = request.SenderInfo.Name,
                    tel = request.SenderInfo.Phone,
                    province = request.SenderInfo.Prov,
                    city = request.SenderInfo.City,
                    area = request.SenderInfo.County,
                    address = request.SenderInfo.Address
                },
                receiver = request.ReceiverInfo == null ? new receiver() : new receiver
                {
                    name = request.ReceiverInfo.Name,
                    tel = request.ReceiverInfo.Phone,
                    province = request.ReceiverInfo.Prov,
                    city = request.ReceiverInfo.City,
                    area = request.ReceiverInfo.County,
                    address = request.ReceiverInfo.Address
                },
                cargo = request.CargoInfo == null ? new cargo() : new cargo
                {
                    battery = "10",
                    goodsType = "小件",
                    goodsName = request.CargoInfo.Name,
                    goodsCount = ParseInt(request.CargoInfo.Count),
                    weight = ParseInt(request.CargoInfo.Weight),
                },
                // 正式环境找网点或市场部申请
                customer = new customer
                {
                    siteCode = setting.CustomerSiteCode,
                    customerName = setting.CustomerName,
                    sitePwd = setting.CustomerSitePassword,
                    monthCustomerCode = setting.MonthCustomerCode
                },
                remark = remark?.ToString(),
                serviceTypeList= new List<string> { "TRACE_PUSH" }
            };
            return content;
        }

        private string GeneratePostData(string apiName, string contentStr, string registApplyKey)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param["api_name"] = apiName;
            param["content"] = contentStr;
            param["from_appkey"] = setting.FromAppkey;
            param["from_code"] = setting.FromCode;
            param["to_appkey"] = registApplyKey;
            param["to_code"] = registApplyKey;
            param["data_digest"] = CalculateDigest(contentStr, setting.SecretKey);

            return Util.CreateLinkString(param, false, true, Encoding.UTF8);
        }
    }
}

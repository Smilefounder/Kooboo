using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Kooboo.Sites.Logistics.Methods.best.Model;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.best.lib
{
    public class BestClient
    {
        private BestSetting setting;
        private const string TraceQuery = "KD_TRACE_QUERY";
        private const string CreateNotify = "KD_CREATE_ORDER_NOTIFY";

        public BestClient(BestSetting setting)
        {
            this.setting = setting;
        }

        public BestCreateOrderResponse CreateOrder(BestCreateOrderRequest request)
        {
            request.orderFlag = "1";//业务类型(0-线下下单,1-线上下单,10-我要寄快递)
            request.serviceType = "1";//服务类型(0 - 线下下单, 1 - 线上下单)
            request.orderType = "1";//订单类型(1-普通订单)
            var body = JsonConvert.SerializeObject(request);
            var result = execute(body, CreateNotify);
            var response = JsonConvert.DeserializeObject<BestCreateOrderResponse>(result);
            if (response.result)
            {
                return response;
            }
            else
            {
                throw new Exception($"Error Status: {response.errorCode}; content: {response.errorDescription}.");
            }
        }

        public BestTraceOrderResponse TraceOrder(string biilCode)
        {
            var mailNos = new MailNos
            {
                mailNo = new List<string> { biilCode }
            };

            var request = new BestTraceOrderRequest
            {
                mailNos = mailNos
            };
            var body = JsonConvert.SerializeObject(request);
            var result = execute(body, TraceQuery);
            var response = JsonConvert.DeserializeObject<BestTraceOrderResponse>(result);
            if (response.result)
            {
                return response;
            }
            else
            {
                throw new Exception($"Error Status: {response.errorCode}; content: {response.errorDescription}.");
            }
        }

        public string execute(string bizData, string serviceType)
        {
            string response = string.Empty;

            SignParams signparams = new SignParams();
            signparams.partnerID = setting.PartnerID;
            signparams.bizData = bizData;
            signparams.serviceType = serviceType;
            signparams.partnerKey = setting.PartnerKey;

            string signString = signparams.bizData + signparams.partnerKey;

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("partnerID", signparams.partnerID);
            data.Add("bizData", signparams.bizData);
            data.Add("serviceType", signparams.serviceType);
            data.Add("sign", SignUtil.MakeMd5Sign(signString));

            string body = BuildQuery(data);
            var resp = ApiClient.Create().PostAsync(setting.ServerURL, body, contentType: "application/x-www-form-urlencoded").Result;

            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception($"Error Status: {resp.StatusCode}; content: {resp.Content}.");
            }

            return resp.Content;
        }

        public string Post(string url, string body, StringBuilder digeStringBuilder)
        {
            var resp = ApiClient.Create().PostAsync(url, body, contentType: "application/x-www-form-urlencoded;charset=utf-8").Result;

            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception($"Error Status: {resp.StatusCode}; content: {resp.Content}.");
            }

            return resp.Content;
        }

        public string BuildQuery(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;

                if (!string.IsNullOrEmpty(name) && value != null)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    hasParam = true;
                }
            }
            return postData.ToString();
        }
    }
}

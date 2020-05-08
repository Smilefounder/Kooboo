using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Kooboo.Sites.Logistics.Methods.deppon.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kooboo.Sites.Logistics.Methods.deppon.lib
{
    public class DEPPONClient
    {
        private DEPPONSetting setting;
        private readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public DEPPONClient(DEPPONSetting setting)
        {
            this.setting = setting;
        }

        public CreateOrderResponse CreateOrder(CreateOrderRequest request)
        {
            request.CompanyCode = setting.CompanyCode;
            request.CustomerCode = setting.CustomerCode;
            request.TransportType = setting.TransportType;
            request.OrderType = "2";

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var content = GenerateQueryBody(JsonConvert.SerializeObject(request, settings));
            string url = "/dop-standard-ewborder/createOrderNotify.action";
            var response = Post(url, content);

            var createOrderResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(response);
            Boolean.TryParse(createOrderResponse.Result, out bool success);
            if (success)
            {
                return createOrderResponse;
            }

            return null;
        }

        public string GetPostage(PostageRequest request)
        {
            request.TotalVolume = "0.001";
            request.LogisticCompanyID = setting.LogisticCompanyID;
            request.SendDateTime = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss");
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var content = GenerateQueryBody(JsonConvert.SerializeObject(request, settings));
            string url = "/standard-order/queryPriceTime.action";
            var response = Post(url, content);

            var postageResponse = JsonConvert.DeserializeObject<PostageResponse>(response);
            Boolean.TryParse(postageResponse.Result, out bool success);
            if (success)
            {
                var fee = postageResponse.ResponseParam.FirstOrDefault(it => string.Equals(it.ProducteCode, setting.TransportType))?.Totalfee;
                return fee;
            }

            return null;
        }

        public Trace TraceOrder(Dictionary<string,string> request)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var content = GenerateQueryBody(JsonConvert.SerializeObject(request, settings));
            string url = "/standard-order/newTraceQuery.action";
            var response = Post(url, content);

            var result = JsonConvert.DeserializeObject<TraceOrderResponse>(response);
            Boolean.TryParse(result.Result, out bool success);
            if (success)
            {
                var traces = result.ResponseParam?.TraceList?.OrderByDescending(it => it.Time);
                if(traces!=null)
                {
                    return traces.FirstOrDefault();
                }
            }

            return null;
        }

        private string Post(string url, string content)
        {
            var resp = ApiClient.Create().PostAsync(setting.ServerURL + url, content, contentType: "application/x-www-form-urlencoded").Result;

            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception($"Error Status: {resp.StatusCode}; content: {resp.Content}.");
            }

            return resp.Content;
        }

        public long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        private string GenerateQueryBody(string body)
        {
            var timestamp = CurrentTimeMillis().ToString();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("companyCode", setting.CompanyCode);
            parameters.Add("timestamp", timestamp);
            var digest = MakeDataDigest(body, timestamp);
            parameters.Add("digest", digest);
            parameters.Add("params", body);
            return BuildQuery(parameters);
        }

        private string MakeDataDigest(string request, string timestamp)
        {
            var md5 = Md5Helper.Md5Hex(request + setting.APPKey + timestamp);
            var bs = Encoding.UTF8.GetBytes(md5);
            var base64 = Convert.ToBase64String(bs);
            return base64;
        }

        private static string BuildQuery(IDictionary<string, string> parameters)
        {
            var postData = new StringBuilder();
            var hasParam = false;

            var dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                var name = dem.Current.Key;
                var value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                        postData.Append("&");

                    postData.Append(name);
                    postData.Append("=");

                    if (name.Equals("params"))
                    {
                        var encodedValue = HttpUtility.UrlEncode(value, Encoding.UTF8);
                        postData.Append(encodedValue);
                    }
                    else
                    {
                        postData.Append(value);
                    }

                    hasParam = true;
                }
            }

            return postData.ToString();
        }
    }
}

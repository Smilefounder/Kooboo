using System;
using System.Collections.Generic;
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

        public string GetPostage(PostageRequest request)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var content = GenerateQueryBody(JsonConvert.SerializeObject(request, settings));
            string url = "http://dpsanbox.deppon.com/sandbox-web/standard-order/queryPrice.action";//string.Format(setting.ServerURL + "/queryPrice.action");
            var result = Post(url, content);

            return result;
        }

        public string Post(string url, string content)
        {
            var resp = ApiClient.Create().PostAsync(url, content, contentType: "application/x-www-form-urlencoded").Result;

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
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(request + setting.APPKey + timestamp));
            var base64 = Convert.ToBase64String(bs);
            return base64;
        }

        //private string MakeDataDigest(string request, string timestamp)
        //{
        //    byte[] body = Encoding.UTF8.GetBytes(request + setting.APPKey + timestamp);
        //    var base64 = Convert.ToBase64String(body);
        //    return base64;
        //}

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

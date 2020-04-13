using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Kooboo.Sites.Logistics.Methods.yto.Model;
using Kooboo.Sites.Logistics.Methods.zop.lib;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.yto.lib
{
    public class YTOClient
    {
        private const string TransportPrice = "yto.Marketing.TransportPrice";
        private readonly YTOSetting setting;

        public YTOClient(YTOSetting setting)
        {
            this.setting = setting;
        }

        public string ChargeQuery(OrderCreateRequest request)
        {
            var result = Post(setting.ServerURL+ "/order_create/v1/U8dQd0", JsonConvert.SerializeObject(request), TransportPrice);

            return result;
        }

        public string ChargeQuery(ChargeQueryRequest request)
        {
            var result = Post(setting.ServerURL+ "/charge_query/v1/U8dQd0", JsonConvert.SerializeObject(new ChargeQueryRequest[] { request }), TransportPrice);

            return result;
        }

        public string Post(string url, string body, string method)
        {
            url = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("user_id", setting.UserId);
            parameters.Add("app_key", setting.AppKey);
            parameters.Add("format", "JSON");
            parameters.Add("method", method);
            parameters.Add("v", setting.Version);
            parameters.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"));
            var sign = MakeSign(parameters, method);
            parameters.Add("sign", sign);
            parameters.Add("param", body);

            string contentType = "application/x-www-form-urlencoded";
            var httpContent = new StringContent(BuildQuery(parameters), null, contentType);
            httpContent.Headers.ContentType.CharSet = null;
            var resp = ApiClient.Create()
                            .SendAsync(HttpMethod.Post, url,
                            httpContent).Result;
            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error Status: {resp.StatusCode}; content: {resp.Content}.");
            }

            return resp.Content;
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

                    if (string.Equals(name, "param"))
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

        private string MakeSign(Dictionary<string, string> paras, string method)
        {
            string pwd = "";
            var parameters = new SortedDictionary<string, string>(paras);

            var generateSign = new StringBuilder();
            foreach (var item in parameters)
            {
                generateSign.Append(item.Key + item.Value);
            }

            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(setting.SecretKey + generateSign.ToString()));
            for (int i = 0; i < bs.Length; i++)
            {
                pwd = pwd + bs[i].ToString("X");
            }
            return pwd;
        }
    }
}

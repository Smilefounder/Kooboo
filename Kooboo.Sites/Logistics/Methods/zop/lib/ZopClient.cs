using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Kooboo.Sites.Logistics.Methods.zop.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kooboo.Sites.Logistics.Methods.zop.lib
{
    public class ZopClient
    {
        private ZOPSetting settings;

        public ZopClient(ZOPSetting settings)
        {
            this.settings = settings;
        }

        public string GetPostage(ZopRequest request)
        {
            ValidatePostageRequest(request);

            var weigth = request.requestParams.Get("weight");
            string url = string.Format("{0}/getPriceForCustomer", settings.ServerURL);
            var result = execute(request, url);

            if (!string.IsNullOrEmpty(result))
            {
                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);

                if (!Convert.ToBoolean(dic["status"]))
                {
                    throw new Exception(result);
                }

                var postage = dic["result"].ToString();

                return postage;
            }

            return "";
        }

        public TraceOrderResponse TraceOrder(string billCode)
        {
            if (string.IsNullOrEmpty(billCode))
            {
                return null;
            }
            string url = string.Format("{0}/traceInterfaceNewTraces", settings.ServerURL);
            var request = new ZopRequest();
            request.addParam("data", JsonConvert.SerializeObject(new[] { billCode }));
            request.addParam("msg_type", "NEW_TRACES");
            request.addParam("company_id", settings.CompanyId);
            var result = execute(request, url);

            if (!string.IsNullOrEmpty(result))
            {
                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);

                if (!Convert.ToBoolean(dic["status"]))
                {
                    throw new Exception(result);
                }

                if (dic["data"] is JArray)
                {
                    var response = JsonConvert.DeserializeObject<List<TraceOrderResponse>>(dic["data"].ToString());
                    return response.FirstOrDefault();
                }
            }

            return null;
        }

        public string CreateOrder(ZopRequest request)
        {
            ValidateCreateOrderRequest(request);

            string url = string.Format("{0}/plateOrder", settings.ServerURL);
            var result = execute(request, url);

            if (!string.IsNullOrEmpty(result))
            {
                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);

                if (!Convert.ToBoolean(dic["status"]))
                {
                    throw new Exception(result);
                }

                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(dic["result"].ToString());
                var billCode = data["billCode"].ToString();

                return billCode;
            }

            return "";
        }

        private void ValidateCreateOrderRequest(ZopRequest request)
        {
            if (string.IsNullOrEmpty(request.requestParams.Get("orderInfo")))
            {
                throw new Exception("orderInfo不能为空！");
            }

            if (string.IsNullOrEmpty(request.requestParams.Get("systemParameter")))
            {
                throw new Exception("systemParameter不能为空！");
            }
        }

        private void ValidatePostageRequest(ZopRequest request)
        {
            if (string.IsNullOrEmpty(request.requestParams.Get("weight")))
            {
                throw new Exception("weight不能为空！");
            }

            if (string.IsNullOrEmpty(request.requestParams.Get("provinceStart")))
            {
                throw new Exception("provinceStart不能为空！");
            }

            if (string.IsNullOrEmpty(request.requestParams.Get("provinceEnd")))
            {
                throw new Exception("provinceEnd不能为空！");
            }
        }


        private string execute(ZopRequest request, string url)
        {
            NameValueCollection requestParams = request.requestParams;
            int i = 0;
            StringBuilder queryStringBuilder = new StringBuilder();
            StringBuilder digeStringBuilder = new StringBuilder();
            foreach (string key in requestParams.Keys)
            {
                if (i > 0)
                {
                    queryStringBuilder.AppendFormat("&{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(requestParams[key], Encoding.UTF8));
                    digeStringBuilder.AppendFormat("&{0}={1}", key, requestParams[key]);
                }
                else
                {
                    queryStringBuilder.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(requestParams[key], Encoding.UTF8));
                    digeStringBuilder.AppendFormat("{0}={1}", key, requestParams[key]);
                }
                i++;
            }


            return Post(url, queryStringBuilder.ToString(), digeStringBuilder);
        }

        public string Post(string url, string body, StringBuilder digeStringBuilder)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("x-companyid", settings.CompanyId);
            string strToDigest = digeStringBuilder + settings.Key;
            headers.Add("x-datadigest", EncryptMD5Base64(strToDigest));

            var resp = ApiClient.Create().PostAsync(url, body, contentType: "application/x-www-form-urlencoded", headers: headers).Result;

            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception($"Error Status: {resp.StatusCode}; content: {resp.Content}.");
            }

            return resp.Content;
        }

        public static string EncryptMD5Base64(string encryptStr, string charset = "UTF-8")
        {
            string rValue = "";
            var m5 = new MD5CryptoServiceProvider();

            byte[] inputBye;
            byte[] outputBye;
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encryptStr);
            }
            catch (Exception)
            {
                inputBye = Encoding.UTF8.GetBytes(encryptStr);
            }
            outputBye = m5.ComputeHash(inputBye);
            rValue = Convert.ToBase64String(outputBye, 0, outputBye.Length);
            return rValue;
        }
    }
}

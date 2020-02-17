using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Kooboo.Lib.Helper;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Kooboo.Web.Api.Implementation.Mails
{
    public class EmailHttpHelper
    {
        public static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 4.0.30319)";

        public static T Get<T>(string url, Dictionary<string, string> query, Dictionary<string, string> headers)
        {
            if (query != null)
            {
                url = UrlHelper.AppendQueryString(url, query);
            }

            Kooboo.Data.Log.Instance.Email.Write("--get--" + url);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url)
            };
            AddHeader(request.Headers, headers);
            var response = HttpHelper.HttpClient.SendAsync(request).Result;
            var backstring = response.Content.ReadAsStringAsync().Result;

            Kooboo.Data.Log.Instance.Email.Write(backstring);

            return ProcessApiResponse<T>(backstring);

        }

        public static T Post<T>(string url, Dictionary<string, string> parameters, Dictionary<string, string> headers)
        {
            try
            {
                var postString = String.Join("&", parameters.Select(it => String.Concat(it.Key, "=", Uri.EscapeDataString(it.Value))));
                var postData = Encoding.UTF8.GetBytes(postString);

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = new ByteArrayContent(postData)
                };
                
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                AddHeader(request.Headers, headers);

                var response = HttpHelper.HttpClient.SendAsync(request).Result;
                var responseData = response.Content.ReadAsByteArrayAsync().Result;

                Kooboo.Data.Log.Instance.Email.Write("--post-- " + url + " -- " + postString);

                var strResult = Encoding.UTF8.GetString(responseData);

                Kooboo.Data.Log.Instance.Email.Write(strResult);

                return ProcessApiResponse<T>(strResult);
            }
            catch (Exception ex)
            {
                Kooboo.Data.Log.Instance.Exception.WriteException(ex);
            }
            return default(T);
        }

        public static T Post<T>(string url, Dictionary<string, string> Headers, byte[] postBytes)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new ByteArrayContent(postBytes)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            AddHeader(request.Headers, Headers);

            try
            {
                var response = HttpHelper.HttpClient.SendAsync(request).Result;
                var responseData = response.Content.ReadAsByteArrayAsync().Result;
                string result = Encoding.UTF8.GetString(responseData);
                Kooboo.Data.Log.Instance.Email.Write("--post--" + url);
                Kooboo.Data.Log.Instance.Email.Write(result);

                return ProcessApiResponse<T>(result);
            }
            catch (Exception ex)
            {
                Kooboo.Data.Log.Instance.Exception.WriteException(ex);
            }
            return default(T);
        }


        public static byte[] PostBytes(string url, byte[] data, Dictionary<string, string> headers)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = new ByteArrayContent(data)
                };
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                AddHeader(request.Headers, headers);
                var response = HttpHelper.HttpClient.SendAsync(request).Result;

                return response.Content.ReadAsByteArrayAsync().Result;
            }
            catch (Exception ex)
            {
                Data.Log.Instance.Email.WriteException(ex);
            }
            return null;
        }

        private static void AddHeader(HttpRequestHeaders webHeaderCollection, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    webHeaderCollection.Add(header.Key, header.Value);
                }
            }
        }

        private static T ProcessApiResponse<T>(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return default(T);
            }

            var jobject = Lib.Helper.JsonHelper.DeserializeJObject(response);

            if (jobject != null)
            {
                var successStr = Lib.Helper.JsonHelper.GetString(jobject, "success");

                var modelstring = Lib.Helper.JsonHelper.GetString(jobject, "Model");

                if (string.IsNullOrWhiteSpace(modelstring) && typeof(T) == typeof(bool))
                {
                    modelstring = successStr;
                }
                if (!string.IsNullOrEmpty(modelstring))
                {
                    var type = typeof(T);

                    if (type.IsClass && type != typeof(string))
                    {
                        return Lib.Helper.JsonHelper.Deserialize<T>(modelstring);
                    }
                    else
                    {
                        return (T)Lib.Reflection.TypeHelper.ChangeType(modelstring, typeof(T));
                    }
                }
            }

            return default(T);
        }
    }
}

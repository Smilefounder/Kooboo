//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Kooboo.Lib.Helper
{
    public class HttpHelper
    {
        private static HttpClient httpClient;
        static HttpHelper()
        {
            //ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;
            ////turn on tls12 and tls11,default is ssl3 and tls
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            SetCustomSslChecker();

            httpClient = CreateHttpClient();
        }

        private static HttpClient CreateHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler();
#if NETSTANDARD2_0
            //ServicePointManager does not affect httpclient in dotnet core
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
#endif
            handler.Proxy = null;
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", DefaultUserAgent);
            return client;
        }

        public static bool HasSetCustomSSL { get; set; }

        public static void SetCustomSslChecker()
        {
            if (!HasSetCustomSSL)
            {
                ServicePointManager.ServerCertificateValidationCallback += CheckValidationResult;
                HasSetCustomSSL = true;
            }
        }


        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //make self signed cert ,so not validate cert in client
            return true;
        }

        public static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 4.0.30319)";

        public static T ProcessApiResponse<T>(string response)
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

        public static T Post<T>(string url, Dictionary<string, string> parameters, string UserName = null, string Password = null)
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
                request.Content.Headers.ContentType =new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                {
                    var bytes = Encoding.UTF8.GetBytes(String.Format("{0}:{1}", UserName, Password));
                    request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(bytes));
                }

                var response = httpClient.SendAsync(request).Result;
                var strResult = Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result);
                return ProcessApiResponse<T>(strResult);


                //using (var client = new WebClient())
                //{
                //    client.Headers.Add("user-agent", DefaultUserAgent);
                //    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                //    if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                //    {
                //        var bytes = Encoding.UTF8.GetBytes(String.Format("{0}:{1}", UserName, Password));
                //        client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(bytes));
                //    }

                //    var responseData = client.UploadData(url, "POST", postData);
                //    var strResult = Encoding.UTF8.GetString(responseData);
                //    return ProcessApiResponse<T>(strResult);
                //}
            }
            catch (Exception ex)
            {
                //TODO: log exception
            }
            return default(T);
        }

        public static T Post<T>(string url, Dictionary<string, string> Headers, byte[] postBytes, string UserName = null, string Password = null)
        {

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new ByteArrayContent(postBytes)
            };
            if (!string.IsNullOrEmpty(UserName))
            {
                if (Headers == null)
                {
                    Headers = new Dictionary<string, string>();
                }
                var bytes = Encoding.UTF8.GetBytes(String.Format("{0}:{1}", UserName, Password));
                Headers.Add("Authorization", "Basic " + Convert.ToBase64String(bytes));
            }
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            if (Headers != null)
            {
                foreach (var item in Headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            try
            {
                var response = httpClient.SendAsync(request).Result;
                var responseData = response.Content.ReadAsByteArrayAsync().Result;
                return ProcessApiResponse<T>(Encoding.UTF8.GetString(responseData));
            }
            catch (Exception ex)
            {

            }
            return default(T);

            //using (var client = new WebClient())
            //{
            //    client.Headers.Add("user-agent", DefaultUserAgent);
            //    client.Headers.Add("Content-Type", "multipart/form-data");
            //    if (Headers != null)
            //    {
            //        foreach (var item in Headers)
            //        {
            //            client.Headers.Add(item.Key, item.Value);
            //        }
            //    }

            //    try
            //    {
            //        var responseData = client.UploadData(url, "POST", postBytes);

            //        return ProcessApiResponse<T>(Encoding.UTF8.GetString(responseData));
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //    return default(T);
            //}
        }

        public static byte[] ConvertKooboo(string url, byte[] data, Dictionary<string, string> headers, string UserName = null, string Password = null)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content=new ByteArrayContent(data)
                };

                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
                if (!string.IsNullOrEmpty(UserName))
                {
                    var bytes = Encoding.UTF8.GetBytes(String.Format("{0}:{1}", UserName, Password));
                    request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(bytes));
                }

                var response = httpClient.SendAsync(request).Result;
                return response.Content.ReadAsByteArrayAsync().Result;

                //using (var client = new WebClient())
                //{
                //    client.Headers.Add("user-agent", DefaultUserAgent);
                //    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                //    foreach (var item in headers)
                //    {
                //        client.Headers.Add(item.Key, item.Value);
                //    }
                //    if (!string.IsNullOrEmpty(UserName))
                //    {
                //        var bytes = Encoding.UTF8.GetBytes(String.Format("{0}:{1}", UserName, Password));
                //        client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(bytes));
                //    }

                //    return client.UploadData(url, "POST", data);
                //}
            }
            catch (Exception ex)
            {
                //TODO: log exception
            }
            return null;
        }

        public static T Post<T>(string url, string json)
        {
            try
            {
                json = System.Net.WebUtility.UrlEncode(json);  ///????? What is this????
                var postData = Encoding.UTF8.GetBytes(json);
                
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = new ByteArrayContent(postData)
                };
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = httpClient.SendAsync(request).Result;
                var responseData = response.Content.ReadAsByteArrayAsync().Result;
                return ProcessApiResponse<T>(Encoding.UTF8.GetString(responseData));

                //using (var client = new WebClient())
                //{
                //    client.Proxy = null;
                //    client.Headers.Add("user-agent", DefaultUserAgent);
                //    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                //    var responseData = client.UploadData(url, "POST", postData);

                //    return ProcessApiResponse<T>(Encoding.UTF8.GetString(responseData));
                //}
            }
            catch (Exception)
            {
                //TODO: log exception
            }
            return default(T);
        }

        public static T Get<T>(string url, Dictionary<string, string> query = null, string UserName = null, string Password = null)
        {
            if (query != null)
            {
                url = UrlHelper.AppendQueryString(url, query);
            }
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };

            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                var bytes = Encoding.UTF8.GetBytes(String.Format("{0}:{1}", UserName, Password));
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(bytes));
            }

            var response = httpClient.SendAsync(request).Result;
            var backstring = response.Content.ReadAsStringAsync().Result;
            return ProcessApiResponse<T>(backstring);

            //using (var client = new WebClient())
            //{
            //    client.Headers.Add("user-agent", DefaultUserAgent);

            //    if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            //    {
            //        var bytes = Encoding.UTF8.GetBytes(String.Format("{0}:{1}", UserName, Password));
            //        client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(bytes));
            //    }
            //    client.Proxy = null;
            //    client.Encoding = Encoding.UTF8;

            //    var backstring1 = client.DownloadString(url);

            //    return ProcessApiResponse<T>(backstring1);
            //}
        }

        public static string GetString(string url)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                var response = httpClient.SendAsync(request).Result;
                return response.Content.ReadAsStringAsync().Result;

                //using (var client = new WebClient())
                //{
                //    client.Headers.Add("user-agent", DefaultUserAgent);

                //    client.Proxy = null;
                //    client.Encoding = Encoding.UTF8;

                //    return client.DownloadString(url);
                //}
            }
            catch (Exception ex)
            {

            }

            return null;
        }


        public static async Task<string> GetStringAsync(string url, Dictionary<string, string> query = null)
        {
            try
            {

                if (query != null)
                {
                    url = UrlHelper.AppendQueryString(url, query);
                }
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                var response = await httpClient.SendAsync(request);
                var data= await response.Content.ReadAsStringAsync();
                return data;

                //using (var client = new WebClient())
                //{
                //    client.Headers.Add("user-agent", DefaultUserAgent);

                //    client.Proxy = null;
                //    client.Encoding = Encoding.UTF8;

                //    return await client.DownloadStringTaskAsync(new Uri(url));
                //}
            }
            catch (Exception ex)
            {

            }

            return null;
        }


        public static T TryGet<T>(string url, Dictionary<string, string> query = null)
        {
            if (query != null)
            {
                url = UrlHelper.AppendQueryString(url, query);
            }

            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                var response = httpClient.SendAsync(request).Result;
                var backstring = response.Content.ReadAsStringAsync().Result;
                return ProcessApiResponse<T>(backstring);

                //using (var client = new WebClient())
                //{
                //    client.Headers.Add("user-agent", DefaultUserAgent);

                //    client.Proxy = null;
                //    client.Encoding = Encoding.UTF8;

                //    var backstring = client.DownloadString(url);

                //    return ProcessApiResponse<T>(backstring);
                //}
            }
            catch (Exception ex)
            {

            }
            return default(T);
        }

        public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null, Dictionary<string, string> query = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return default(T);
            }
            if (query != null)
            {
                url = UrlHelper.AppendQueryString(url, query);
            }

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            var response = await httpClient.SendAsync(request);
            var backstring = await response.Content.ReadAsStringAsync();
            return ProcessApiResponse<T>(backstring);

            //using (var client = new WebClient())
            //{
            //    client.Headers.Add("user-agent", DefaultUserAgent);

            //    if (headers != null)
            //    {
            //        foreach (var item in headers)
            //        {
            //            client.Headers.Add(item.Key, item.Value);
            //        }
            //    }

            //    if (query != null)
            //    {
            //        url = UrlHelper.AppendQueryString(url, query);
            //    }

            //    client.Proxy = null;
            //    client.Encoding = Encoding.UTF8;

            //    var backstring = await client.DownloadStringTaskAsync(new Uri(url));
            //    var result = ProcessApiResponse<T>(backstring);
            //    return result;
            //}
        }


        public static async Task<T> TryGetAsync<T>(string url, Dictionary<string, string> headers = null, Dictionary<string, string> query = null)
        {

            try
            {
                return await GetAsync<T>(url, headers, query);
            }
            catch (Exception ex)
            {
                return default(T);
            }


        }


        public static bool PostData(string url, Dictionary<string, string> Headers, byte[] PostBytes, string UserName = null, string Password = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content=new ByteArrayContent(PostBytes)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            if (!string.IsNullOrEmpty(UserName))
            {
                if (Headers == null)
                {
                    Headers = new Dictionary<string, string>();
                }
                var bytes = Encoding.UTF8.GetBytes(String.Format("{0}:{1}", UserName, Password));
                Headers.Add("Authorization", "Basic " + Convert.ToBase64String(bytes));
            }
           
            if (Headers != null)
            {
                foreach (var item in Headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            bool success = false;
            try
            {
                var response = httpClient.SendAsync(request).Result;
                var responseData = response.Content.ReadAsByteArrayAsync().Result;
                var ok = ProcessApiResponse<bool>(Encoding.UTF8.GetString(responseData));

                success = ok;
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;

            //using (var client = new WebClient())
            //{
            //    client.Headers.Add("user-agent", DefaultUserAgent);
            //    client.Headers.Add("Content-Type", "multipart/form-data");
            //    if (Headers != null)
            //    {
            //        foreach (var item in Headers)
            //        {
            //            client.Headers.Add(item.Key, item.Value);
            //        }
            //    }

            //    bool success = false;
            //    try
            //    {
            //        var responseData = client.UploadData(url, "POST", PostBytes);

            //        var ok = ProcessApiResponse<bool>(Encoding.UTF8.GetString(responseData));

            //        success = ok;
            //    }
            //    catch (Exception ex)
            //    {
            //        success = false;
            //    }
            //    return success;
            //}
        }

    }
}
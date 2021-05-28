using Kooboo.Sites.OpenApi.Senders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Kooboo.Sites.OpenApi
{
    public abstract class HttpSender
    {
        static readonly List<HttpSender> _senders = Lib.IOC.Service.GetInstances<HttpSender>();

        protected abstract string ContentType { get; }
        protected abstract byte[] SerializeBody(object body, HttpWebRequest request);

        public static HttpSender GetSender(string contentType)
        {
            var sender = _senders.FirstOrDefault(f => contentType.Contains(f.ContentType));
            if (sender == null) sender = _senders.First(f => f is Json);
            return sender;
        }
        public static string ErrorFieldName => "errorMsg";

        public object Send(string url, string method, object body, IDictionary<string, string> headers, IDictionary<string, string> cookies)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ContentType = ContentType;

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    AddHeader(request, item.Key, item.Value);
                }
            }

            if (cookies != null)
            {
                foreach (var item in cookies)
                {
                    AddCookie(request, item.Key, item.Value);
                }
            }

            if (body != null)
            {
                var data = SerializeBody(body, request);
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    var handler = ResponseHandler.Get(response.ContentType);
                    using (var stream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(stream);
                        return handler.Handler(reader.ReadToEnd());
                    }
                }
            }
            catch (WebException e)
            {
                var response = e.Response as HttpWebResponse;
                object errorBody = null;

                if (response != null)
                {
                    var handler = ResponseHandler.Get(response.ContentType);

                    using (var stream = e.Response.GetResponseStream())
                    {
                        var reader = new StreamReader(stream);
                        errorBody = handler.Handler(reader.ReadToEnd());
                    }
                }

                return new Dictionary<string, object>
                {
                    {"code",response==null?500: (int)response?.StatusCode},
                    {"body", errorBody },
                    {ErrorFieldName,e.Message }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void AddCookie(HttpWebRequest request, string name, string value)
        {
            if (request.CookieContainer == null) request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(name, value));
        }

        protected void AddHeader(HttpWebRequest request, string name, string value)
        {
            request.Headers.Add(name, value);
        }
    }
}

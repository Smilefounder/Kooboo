using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Kooboo.Sites.Logistics.Methods.sf.Model;

namespace Kooboo.Sites.Logistics.Methods.sf.lib
{
    public class SFClient
    {
        private const string OrderService = "OrderService";
        private const string RouteService = "RouteService";
        private SFSetting setting = null;
        private XmlWriterSettings settings = null;
        private XmlSerializerNamespaces ns = null;

        public SFClient(SFSetting setting)
        {
            this.setting = setting;
            settings = new XmlWriterSettings()
            {
                Encoding = new UnicodeEncoding(false, false), // no BOM in a .NET string
                Indent = false,
                OmitXmlDeclaration = true
            };
            ns = new XmlSerializerNamespaces();
            ns.Add("", "");
        }

        public CreateOrderResponse CreateOrder(CreateOrderRequest request)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = new UnicodeEncoding(false, false), // no BOM in a .NET string
                Indent = false,
                OmitXmlDeclaration = true
            };

            var body = XmlSerializerUtilis.SerializeXML(settings, ns, request);
            var xml = GenerateRequestXML(OrderService, body);
            var result = Post(setting.ServerURL, xml);

            if (result != null)
            {
                var reponse = XmlSerializerUtilis.DeserializeXML<CreateOrderResponse>(result);
                if (reponse.Head == "OK")
                {
                    return reponse;
                }
            }

            return null;
        }

        public Route TraceOrder(TraceOrderRequest request)
        {
            request.MethodType = "1";
            request.TrackingType = "1"; //如果tracking_type = 1,则此值为顺丰运单号 如果tracking_type = 2, 则此值为客户订单号 如果tracking_type = 3,则此值为逆向单原始订单号
              var body = XmlSerializerUtilis.SerializeXML(settings, ns, request);
            var xml = GenerateRequestXML(RouteService, body);
            var result = Post(setting.ServerURL, xml);

            if (result != null)
            {
                var response = XmlSerializerUtilis.DeserializeXML<TraceOrderResponse>(result);
                if (response.Head == "OK")
                {
                    return response.Body.Route.OrderByDescending(it => it.AcceptTime)?.FirstOrDefault();
                }
            }

            return null;
        }

        public string Post(string url, string body)
        {
            string verifyCode = VerifyCodeUtil.md5EncryptAndBase64(body + setting.CheckWord);

            string contentType = "application/x-www-form-urlencoded";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}={1}", "xml", (object)body);
            stringBuilder.Append("&");
            stringBuilder.AppendFormat("{0}={1}", (object)nameof(verifyCode), (object)verifyCode);
            var httpContent = new StringContent(stringBuilder.ToString(), Encoding.UTF8, contentType);
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

        private string GenerateRequestXML(string serviceType, string body)
        {
            string xml = string.Format("<Request service='{0}' lang='{1}'><Head>{2}</Head>{3}</Request>",
                          serviceType, setting.Lang, setting.ClientCode, body);
            return xml;
        }
    }
}

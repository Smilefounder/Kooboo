using System;
using System.Collections.Generic;
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
        public SFSetting setting;

        public SFClient(SFSetting setting)
        {
            this.setting = setting;
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

            var body = XmlSerializerUtilis.SerializeXML<CreateOrderRequest>(settings, ns, request);
            var xml = GenerateRequestXML(OrderService, body);
            var result = Post(setting.ServerURL, xml);

            if (result != null)
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                var reponse = XmlSerializerUtilis.DeserializeXML<CreateOrderResponse>(readerSettings, result);
                if (reponse.Head == "OK")
                {
                    return reponse;
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

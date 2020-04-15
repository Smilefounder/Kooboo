using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Kooboo.Sites.Logistics.Methods.yto.Model;
using Kooboo.Sites.Logistics.Methods.zop.lib;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.yto.lib
{
    public class YTOClient
    {
        private const string TransportPrice = "yto.Marketing.TransportPrice";
        private const string BillTrace = "yto.Marketing.WaybillTrace";
        private readonly YTOSetting setting;

        public YTOClient(YTOSetting setting)
        {
            this.setting = setting;
        }

        public OrderCreateResponse TraceOrder(OrderTraceRequest request)
        {
            var content = GenerateQueryBody(JsonConvert.SerializeObject(new OrderTraceRequest[] { request }), BillTrace);
            var result = Post(setting.ServerURL + "/waybill_query/v1/U8dQd0", content);
            var orderModel = JsonConvert.DeserializeObject<List<OrderCreateResponse>>(result);
            return orderModel.OrderByDescending(it => it.Upload_Time).FirstOrDefault();
        }

        public CreateOrderResponse CreateOrder(RequestOrder request)
        {
            request.ClientID = setting.ClientID;
            request.Items = new List<Item>
            {
                new Item
                {
                    ItemName ="0",
                    Number = 0
                }
            };
            request.OrderType = "1";//订单类型(0-COD,1-普通订单,3-退货单)
            request.ServiceType = "0";//服务类型(1-上门揽收, 2-次日达 4-次晨达 8-当日达,0-自己联系)。（数据库未使用）（目前暂未使用默认为0）
            request.Special = "0";//商品类型（保留字段，暂时不用，默认填0）
            request.LogisticProviderID = setting.LogisticProviderID + "11";
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); ns.Add("", "");

            XmlSerializer serializer = new XmlSerializer(typeof(RequestOrder));

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = new UnicodeEncoding(false, false), // no BOM in a .NET string
                Indent = false,
                OmitXmlDeclaration = true
            };
            string xml = "";
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, request, ns);
                }
                xml = textWriter.ToString();
            }

            var content = GenerateCreateBody(xml);
            var result = Post(setting.ServerURL + "/order_create/v1/U8dQd0", content);//setting.ServerURL + "/order_create/v1/U8dQd0", content);

            return DeserializeResponse(result);
        }

        public string ChargeQuery(ChargeQueryRequest request)
        {
            var content = GenerateQueryBody(JsonConvert.SerializeObject(new ChargeQueryRequest[] { request }), TransportPrice);
            var result = Post(setting.ServerURL + "/charge_query/v1/U8dQd0", content);

            return result;
        }

        public string Post(string url, string content)
        {
            string contentType = "application/x-www-form-urlencoded";
            var httpContent = new StringContent(content, null, contentType);
            httpContent.Headers.ContentType.CharSet = null;
            var resp = ApiClient.Create()
                            .SendAsync(HttpMethod.Post, url,
                            httpContent).Result;
            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error Status: {resp.StatusCode}; content: {resp.Content}.");
            }

            var result = DeserializeResponse(resp.Content);
            if (!result.Success)
            {
                throw new HttpRequestException($"content: {result.Reason}.");
            }

            return resp.Content;
        }

        public CreateOrderResponse DeserializeResponse(string response)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CreateOrderResponse));

            XmlReaderSettings settings = new XmlReaderSettings();
            // No settings need modifying here

            using (StringReader textReader = new StringReader(response))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (CreateOrderResponse)serializer.Deserialize(xmlReader);
                }
            }
        }

        private string GenerateCreateBody(string body)
        {
            var encodeValue = HttpUtility.UrlEncode(body, Encoding.UTF8);
            var dic = new Dictionary<string, string>();
            dic.Add("logistics_interface", encodeValue);
            dic.Add("data_digest", MakeDataDigest(body));
            dic.Add("type", "online");
            dic.Add("clientId", "TEST");//setting.ClientID);
            return BuildQuery(dic);
        }

        private string GenerateQueryBody(string body, string method)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("user_id", setting.UserId);
            parameters.Add("app_key", setting.AppKey);
            parameters.Add("format", "JSON1");
            parameters.Add("method", method);
            parameters.Add("v", setting.Version);
            parameters.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"));
            var sign = MakeSign(parameters, method);
            parameters.Add("sign", sign);
            parameters.Add("param", body);
            return BuildQuery(parameters);
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

        private string MakeDataDigest(string request)
        {
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(request + "123456"));//setting.PartnerID));
            var base64 = Convert.ToBase64String(bs);
            return HttpUtility.UrlEncode(base64, Encoding.UTF8);
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

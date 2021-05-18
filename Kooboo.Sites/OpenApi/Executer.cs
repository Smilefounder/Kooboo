using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Json;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Scripting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace Kooboo.Sites.OpenApi
{
    public class Executer : FunctionInstance
    {
        readonly string _openApiName;
        readonly string _pathName;
        readonly RenderContext _renderContext;

        public Executer(string openApiName, string pathName, RenderContext renderContext)
            : base(Manager.GetJsEngine(renderContext), null, null, false)
        {
            _openApiName = openApiName;
            _pathName = pathName;
            _renderContext = renderContext;
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            var cache = Cache.Get(_renderContext.WebSite);
            cache.Documents.TryGetValue(_openApiName, out var doc);
            if (doc == null) throw new Exception($"Can not found openApi {_openApiName}");
            doc.Operations.TryGetValue(_pathName, out var operation);
            if (operation == null) throw new Exception($"Can not found openApi operation {_pathName}");
            return Send(operation, arguments);
        }

        JsValue Send(Operation operation, JsValue[] arguments)
        {
            var url = operation.Url.ToLower();
            var queue = new Queue<JsValue>(arguments);
            StringContent content;

            if (operation.Body == null)
            {
                content = new StringContent("");
            }
            else
            {
                var json = JsonHelper.Serialize(queue.Dequeue().ToObject());
                content = new StringContent(json, Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            }

            if (operation.Querys.Any()) url = AppendQueryString(url, queue.Dequeue());
            if (operation.Paths.Any()) url = FillUrl(url, queue.Dequeue());
            if (operation.Headers.Any()) FillHeader(content.Headers, queue.Dequeue());
            // if (operation.Cookies.Any()) url = FillUrl(url, queue.Dequeue());


            var response = HttpClientHelper.Client.SendAsync(new HttpRequestMessage
            {
                Method = operation.Method,
                RequestUri = new Uri(url),
                Content = content
            }).Result;

            var byteArray = response.Content.ReadAsByteArrayAsync().Result;
            var result = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            if (JsonHelper.IsJson(result)) return new JsonParser(Engine).Parse(result);
            return JsValue.FromObject(Engine, result);
        }

        private void FillHeader(HttpContentHeaders headers, JsValue jsValue)
        {
            var dic = jsValue.ToObject() as IDictionary<string, object>;

            foreach (var item in dic)
            {
                headers.TryAddWithoutValidation(item.Key, item.Value.ToString());
            }
        }

        private string AppendQueryString(string url, JsValue jsValue)
        {
            var dic = jsValue.ToObject() as IDictionary<string, object>;
            return UrlHelper.AppendQueryString(url, dic.ToDictionary(k => k.Key, v => v.Value.ToString()));
        }

        private string FillUrl(string url, JsValue jsValue)
        {
            var dic = jsValue.ToObject() as IDictionary<string, object>;

            foreach (var item in dic)
            {
                url = url.Replace($"{{{item.Key.ToLower()}}}", item.Value.ToString());
            }

            return url;
        }
    }
}

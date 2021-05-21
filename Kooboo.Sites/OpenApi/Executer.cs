using Jint.Native;
using Jint.Native.Function;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Scripting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Sites.OpenApi
{
    public class Executer : FunctionInstance
    {
        readonly string _openApiName;
        readonly string _pathName;
        readonly RenderContext _renderContext;
        public static string DefaultContentType => "application/json";

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
            return Send(doc, operation, arguments);
        }

        JsValue Send(Document doc, Operation operation, JsValue[] arguments)
        {
            var url = operation.Url.ToLower();
            var queue = new Queue<JsValue>(arguments);
            object body = null;

            Dictionary<string, string> querys = null;
            Dictionary<string, string> paths = null;
            Dictionary<string, string> headers = null;
            Dictionary<string, string> cookies = null;

            if (operation.Body != null) body = queue.Dequeue().ToObject();
            if (operation.Querys.Any()) querys = ToDictionary(queue.Dequeue());
            if (operation.Paths.Any()) paths = ToDictionary(queue.Dequeue());
            if (operation.Headers.Any()) headers = ToDictionary(queue.Dequeue());
            if (operation.Cookies.Any()) cookies = ToDictionary(queue.Dequeue());

            if (operation.Security != null)
            {
                var name = operation.Security.Reference?.Id;
                doc.Securities.TryGetValue(name, out var data);
                if (data == null) throw new Exception($"Not security {name} settings");
                var security = Security.Get(operation.Security.Type);
                var securityResult = security.Authorize(operation.Security, data);
                querys = MergeSecurity(querys, securityResult.Querys);
                headers = MergeSecurity(headers, securityResult.Headers);
                cookies = MergeSecurity(cookies, securityResult.Cookies);
            }

            if (paths != null) url = FillUrl(url, paths);
            if (querys != null) url = UrlHelper.AppendQueryString(url, querys);
            var requestContentType = GetRequestContentType(operation);
            var str = HttpSender.GetSender(requestContentType).Send(url, operation.Method, body, headers, cookies);
            var responseContentType = GetResponseContentType(operation);
            var result = ResponseHandler.Get(responseContentType).Handler(str);
            return JsValue.FromObject(Engine, result);
        }

        private string GetRequestContentType(Operation operation)
        {
            if ((operation.Body?.Content?.Count ?? 0) == 0) return DefaultContentType;
            if (operation.Body.Content.Any(f => f.Key == DefaultContentType)) return DefaultContentType;
            return operation.Body.Content.FirstOrDefault().Key;
        }

        private string GetResponseContentType(Operation operation)
        {
            if ((operation.Responses?.Count ?? 0) == 0) return DefaultContentType;

            if (!operation.Responses.TryGetValue("200", out OpenApiResponse response))
            {
                response = operation.Responses.First().Value;
            }

            if (response.Content.Any(f => f.Key == DefaultContentType)) return DefaultContentType;
            return response.Content.FirstOrDefault().Key;
        }

        private static Dictionary<string, string> MergeSecurity(Dictionary<string, string> request, Dictionary<string, string> security)
        {
            if (security != null)
            {
                if (request == null) request = new Dictionary<string, string>();

                foreach (var item in security)
                {
                    if (!request.ContainsKey(item.Key)) request.Add(item.Key, item.Value);
                }
            }

            return request;
        }


        Dictionary<string, string> ToDictionary(JsValue value)
        {
            if (value == null) return null;
            var obj = value.ToObject() as IDictionary<string, object>;
            if (obj == null) return null;
            return obj.ToDictionary(o => o.Key, o => o.Value.ToString());
        }

        private string FillUrl(string url, IDictionary<string, string> dic)
        {
            foreach (var item in dic)
            {
                url = url.Replace($"{{{item.Key.ToLower()}}}", item.Value);
            }

            return url;
        }
    }
}

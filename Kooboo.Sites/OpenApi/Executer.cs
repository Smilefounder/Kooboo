using Jint.Native;
using Jint.Native.Function;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Scripting;
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
            return Send(operation, arguments);
        }

        JsValue Send(Operation operation, JsValue[] arguments)
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

            if (paths != null) url = FillUrl(url, paths);
            if (querys != null) url = UrlHelper.AppendQueryString(url, querys);

            var contentType = GetContentType(operation);

            if (operation.Security != null) {
                
            }

            var result = HttpSender.GetSender(contentType).Send(url, operation.Method, body, headers, cookies);
            return JsValue.FromObject(Engine, result);
        }

        private string GetContentType(Operation operation)
        {
            if ((operation.Body?.Content?.Count ?? 0) == 0) return DefaultContentType;
            var isJson = operation.Body.Content.Any(f => f.Key == DefaultContentType);
            if (isJson) return DefaultContentType;
            return operation.Body.Content.FirstOrDefault().Key;
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

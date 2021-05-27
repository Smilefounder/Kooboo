using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Extensions;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.OpenApi
{
    public class Operation
    {
        readonly Document _doc;
        readonly Lazy<string> _description;
        readonly Lazy<string> _contentType;
        readonly Lazy<string> _url;
        readonly string _method;
        readonly KeyValuePair<string, OpenApiPathItem> _path;
        readonly KeyValuePair<OperationType, OpenApiOperation> _operation;
        readonly Models.OpenApi.Cache _cache;
        readonly ConcurrentDictionary<string, Tuple<DateTime, object>> _caches = new ConcurrentDictionary<string, Tuple<DateTime, object>>();
        public static string DefaultContentType => "application/json";

        public string Description => _description.Value;
        public OpenApiResponses Responses => _operation.Value.Responses;
        public OpenApiSecurityScheme Security { get; private set; }
        public OpenApiRequestBody Body => _operation.Value.RequestBody;
        public OpenApiParameter[] Querys { get; }
        public OpenApiParameter[] Paths { get; }
        public OpenApiParameter[] Headers { get; }
        public OpenApiParameter[] Cookies { get; }

        public Operation(Document doc, KeyValuePair<string, OpenApiPathItem> path, KeyValuePair<OperationType, OpenApiOperation> operation)
        {
            _doc = doc;
            _path = path;
            _operation = operation;
            _method = _operation.Key.ToString();
            _url = new Lazy<string>(() => new Uri(new Uri(doc.Server), _path.Key).ToString().ToLower(), true);
            _contentType = new Lazy<string>(GetContentType, true);
            _description = new Lazy<string>(GetDescription, true);

            _cache = _doc.OpenApi.Caches.Where(w => w.Method == _method && _path.Key.Contains(w.Pattern) && w.ExpiresIn != 0)
                                        .OrderByDescending(o => o.ExpiresIn)
                                        .FirstOrDefault();

            Querys = _operation.Value.Parameters.Where(w => w.In == ParameterLocation.Query).ToArray();
            Paths = _operation.Value.Parameters.Where(w => w.In == ParameterLocation.Path).ToArray();
            Headers = _operation.Value.Parameters.Where(w => w.In == ParameterLocation.Header).ToArray();
            Cookies = _operation.Value.Parameters.Where(w => w.In == ParameterLocation.Cookie).ToArray();
            Security = operation.Value.Security?.FirstOrDefault()?.FirstOrDefault().Key;
        }

        // params order [body,query,patch,header,cookie]
        public object Send(RenderContext context, IEnumerable<object> @params)
        {
            var url = _url.Value;
            var queue = new ConcurrentQueue<object>(@params);
            object body = null;
            if (Body != null) queue.TryDequeue(out body);
            Dictionary<string, string> querys = Querys.Any() ? GetParams(queue) : null;
            Dictionary<string, string> paths = Paths.Any() ? GetParams(queue) : null;
            Dictionary<string, string> headers = Headers.Any() ? GetParams(queue) : null;
            Dictionary<string, string> cookies = Cookies.Any() ? GetParams(queue) : null;

            if (Security != null)
            {
                var name = Security.Reference?.Id;
                _doc.OpenApi.Securities.TryGetValue(name, out var data);
                if (data == null) throw new Exception($"Not security {name} settings");
                var security = OpenApi.Security.Get(Security.Type);
                var securityResult = security.Authorize(Security, data);
                querys = MergeSecurity(querys, securityResult.Querys);
                headers = MergeSecurity(headers, securityResult.Headers);
                cookies = MergeSecurity(cookies, securityResult.Cookies);
                if (securityResult.ShouldSaveData) context.WebSite.SiteDb().OpenApi.AddOrUpdate(_doc.OpenApi);
            }

            if (paths != null) url = FillUrl(url, paths);
            if (querys != null) url = UrlHelper.AppendQueryString(url, querys);
            var sender = HttpSender.GetSender(_contentType.Value);
            return sender.Send(url, _method, body, headers, cookies);
        }

        string GetDescription()
        {
            var sb = new StringBuilder();
            //TODO add params response ...

            if (_operation.Value.Description != null)
            {
                sb.AppendLine($"Description: {_operation.Value.Description}");
            }

            return sb.ToString();
        }

        static Dictionary<string, string> GetParams(ConcurrentQueue<object> queue)
        {
            if (!queue.TryDequeue(out var value)) return null;
            if (!(value is IDictionary<string, object>)) return null;
            return (value as IDictionary<string, object>).ToDictionary(o => o.Key, o => o.Value.ToString());
        }

        static string FillUrl(string url, IDictionary<string, string> dic)
        {
            foreach (var item in dic)
            {
                url = url.Replace($"{{{item.Key.ToLower()}}}", item.Value);
            }

            return url;
        }

        string GetContentType()
        {
            if ((_operation.Value.RequestBody?.Content?.Count ?? 0) == 0) return DefaultContentType;
            if (_operation.Value.RequestBody.Content.Any(f => f.Key == DefaultContentType)) return DefaultContentType;
            return _operation.Value.RequestBody.Content.FirstOrDefault().Key;
        }

        static Dictionary<string, string> MergeSecurity(Dictionary<string, string> request, Dictionary<string, string> security)
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
    }
}

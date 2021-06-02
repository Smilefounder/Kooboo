using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.Sites.OpenApi
{
    public class Document
    {
        readonly Models.OpenApi _openApi;
        readonly Lazy<OpenApiDocument> _doc;
        readonly Lazy<string> _server;
        readonly Lazy<string> _description;
        readonly Lazy<IDictionary<string, OpenApiSchema>> _schemas;
        readonly Lazy<Dictionary<string, Operation>> _operations;

        public Models.OpenApi OpenApi => _openApi;
        public string Server => _server.Value;
        public string Description => _description.Value;
        public IDictionary<string, OpenApiSchema> Schemas => _schemas.Value;
        public Dictionary<string, Operation> Operations => _operations.Value;

        public Document(Models.OpenApi openApi)
        {
            _openApi = openApi;
            _doc = new Lazy<OpenApiDocument>(() => new OpenApiStringReader().Read(openApi.JsonData, out _), true);
            _schemas = new Lazy<IDictionary<string, OpenApiSchema>>(() => _doc.Value.Components?.Schemas, true);
            _server = new Lazy<string>(GetServer, true);
            _description = new Lazy<string>(GetDescription, true);
            _operations = new Lazy<Dictionary<string, Operation>>(GetOperations, true);
        }

        Dictionary<string, Operation> GetOperations()
        {
            if (_doc.Value.Paths == null) return null;
            var result = new Dictionary<string, Operation>();

            foreach (var path in _doc.Value.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    var key = Helpers.StandardPath(path.Key, operation.Key);
                    if (!result.ContainsKey(key))
                    {

                        result.Add(key, new Operation(this, path, operation));
                    }
                }
            }

            return result;
        }

        string GetServer()
        {
            if (!string.IsNullOrWhiteSpace(_openApi.BaseUrl)) return _openApi.BaseUrl;
            var url = _doc.Value.Servers.FirstOrDefault()?.Url ?? "/";
            Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri);
            if (uri.IsAbsoluteUri) return url;
            return new Uri(new Uri(TryGetBaseUrl()), uri).ToString();
        }

        string TryGetBaseUrl()
        {
            if (string.IsNullOrWhiteSpace(_openApi.Url)) throw new Exception("OpenApi base url can not empty");
            var uri = new Uri(_openApi.Url);
            return $"{uri.Scheme}://{uri.Authority}";
        }

        string GetDescription()
        {
            var sb = new StringBuilder();
            var info = _doc.Value.Info;
            var servers = _doc.Value.Servers;

            if (info != null)
            {
                sb.AppendLine($"[Info]");
                sb.AppendLine($"Title: {info.Title}");
                sb.AppendLine($"Contact: {info.Contact}");
                sb.AppendLine($"Version: {info.Version}");
                sb.AppendLine($"Description: {info.Description}");
                sb.AppendLine(" ");
            }

            if (servers != null)
            {
                sb.AppendLine($"[Servers]");

                foreach (var item in servers)
                {
                    sb.AppendLine(item.Description);
                    sb.AppendLine(item.Url);
                    sb.AppendLine();
                }

                sb.AppendLine(" ");
            }

            return sb.ToString();
        }
    }
}

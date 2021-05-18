using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

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

        private Dictionary<string, Operation> GetOperations()
        {
            if (_doc.Value.Paths == null) return null;
            var result = new Dictionary<string, Operation>();

            foreach (var path in _doc.Value.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    var key = Helpers.StandardPath(path.Key, operation.Key);
                    if (!result.ContainsKey(key)) result.Add(key, new Operation(Server, path, operation));
                }
            }

            return result;
        }

        string GetServer()
        {
            var uri = new Uri(_openApi.Url);
            var baseUrl = $"{uri.Scheme}://{uri.Authority}";
            var url = _doc.Value.Servers.FirstOrDefault()?.Url;
            if (url == null) return baseUrl;
            Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri);
            if (uri.IsAbsoluteUri) return url;
            return $"{baseUrl}/{url}";
        }

        string GetDescription()
        {
            var sb = new StringBuilder();
            var info = _doc.Value.Info;

            if (info != null)
            {
                sb.AppendLine($"Title: {info.Title}");
                sb.AppendLine($"Contact: {info.Contact}");
                sb.AppendLine($"Version: {info.Version}");
                sb.AppendLine($"Description: {info.Description}");
            }

            //TODO add auth server ...

            return sb.ToString();
        }
    }
}

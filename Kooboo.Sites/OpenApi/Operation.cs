using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.OpenApi
{
    public class Operation
    {
        readonly Lazy<string> _description;
        private KeyValuePair<string, OpenApiPathItem> _path;
        private KeyValuePair<OperationType, OpenApiOperation> _operation;

        public string Description => _description.Value;
        public OpenApiResponses Responses => _operation.Value.Responses;
        public string Method { get; private set; }

        public IEnumerable<OpenApiParameter> Paths => _operation.Value.Parameters.Where(w => w.In == ParameterLocation.Path);
        public IEnumerable<OpenApiParameter> Querys => _operation.Value.Parameters.Where(w => w.In == ParameterLocation.Query);
        public IEnumerable<OpenApiParameter> Headers => _operation.Value.Parameters.Where(w => w.In == ParameterLocation.Header);
        public IEnumerable<OpenApiParameter> Cookies => _operation.Value.Parameters.Where(w => w.In == ParameterLocation.Cookie);
        public OpenApiRequestBody Body => _operation.Value.RequestBody;
        public string Url { get; private set; }
        public OpenApiSecurityScheme Security { get; private set; }

        public Operation(string baseUrl, KeyValuePair<string, OpenApiPathItem> path, KeyValuePair<OperationType, OpenApiOperation> operation)
        {
            _path = path;
            _operation = operation;
            _description = new Lazy<string>(GetDescription, true);
            Method = _operation.Key.ToString();
            Url = new Uri(new Uri(baseUrl), _path.Key).ToString();
            Security = operation.Value.Security?.FirstOrDefault()?.FirstOrDefault().Key;
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
    }
}

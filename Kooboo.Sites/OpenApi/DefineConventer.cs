using Kooboo.Sites.Scripting.KDefine;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Sites.OpenApi
{
    public class DefineConventer : IDefineConventer
    {
        readonly Document _doc;
        readonly string _name;
        readonly string _namespace;

        public DefineConventer(Document doc, string name, string @namespace)
        {
            _doc = doc;
            _name = name;
            _namespace = @namespace;
        }

        public Define[] Convent()
        {
            var defines = new List<Define>();

            var define = new Define
            {
                Namespace = _namespace,
                Name = _name,
                Methods = new List<Define.Method>()
            };

            if (_doc.Schemas != null) AddModels(defines, _doc.Schemas);
            if (_doc.Operations != null) AddMethods(defines, define.Methods, _doc.Operations);
            defines.Add(define);
            return defines.ToArray();
        }
        void AddModels(List<Define> defines, IDictionary<string, OpenApiSchema> schemas)
        {
            foreach (var schema in schemas)
            {
                var define = new Define
                {
                    Namespace = _namespace,
                    Name = Helpers.StandardName(schema.Key),
                    Discription = schema.Value.Description,
                    Properties = new List<Define.Property>()
                };

                AddProperties(define.Properties, schema.Value.Properties);
                defines.Add(define);
            }
        }
        void AddProperties(List<Define.Property> properties, IDictionary<string, OpenApiSchema> schemas)
        {
            foreach (var schema in schemas)
            {
                var property = new Define.Property
                {
                    Name = NullableWrap(schema.Key, schema.Value),
                    Discription = schema.Value.Description,
                    Type = TypeMapping(schema.Value)
                };

                properties.Add(property);
            }
        }
        static string NullableWrap(string name, OpenApiSchema schema)
        {
            return name + (schema.Nullable ? "?" : string.Empty);
        }
        static string NullableWrap(string name, bool nullable)
        {
            return name + (nullable ? "?" : string.Empty);
        }
        static string TypeMapping(OpenApiSchema schema)
        {
            if (schema == null) return "any";

            try
            {
                switch (schema.Type)
                {
                    case "number":
                    case "integer":
                        return "number";
                    case "boolean":
                        return "boolean";
                    case "string":
                        if (schema.Format?.StartsWith("date") ?? false) return "Date";
                        return "string";
                    case "array":
                        return $"{TypeMapping(schema.Items)}[]";
                    case null:
                        return TypeMapping(schema.OneOf?.FirstOrDefault() ?? schema.AllOf?.FirstOrDefault() ?? schema.AnyOf?.FirstOrDefault());
                    default:
                        return Helpers.StandardName(schema.Reference?.Id ?? TypeMapping(schema.AdditionalProperties));
                }
            }
            catch (System.Exception)
            {
                return "any";
            }
        }
        void AddMethods(List<Define> defines, List<Define.Method> methods, Dictionary<string, Operation> operations)
        {
            foreach (var operation in operations)
            {
                methods.Add(new Define.Method
                {
                    Name = operation.Key,
                    ReturnType = GetResponse(operation.Value.Responses),
                    Discription = operation.Value.Description,
                    Params = GetParams(defines, operation.Key, operation.Value)
                });
            }
        }
        List<Define.Method.Param> GetParams(List<Define> defines, string name, Operation operation)
        {
            var @params = new List<Define.Method.Param>();

            if (operation.Body != null)
            {
                @params.Add(new Define.Method.Param
                {
                    Name = "body",
                    Type = GetBody(operation.Body.Content)
                });
            }

            if (operation.Querys.Any()) AddParam("query", defines, name, operation.Querys, @params);
            if (operation.Paths.Any()) AddParam("path", defines, name, operation.Paths, @params);
            if (operation.Headers.Any()) AddParam("header", defines, name, operation.Headers, @params);
            if (operation.Cookies.Any()) AddParam("cookie", defines, name, operation.Cookies, @params);

            return @params;
        }

        private void AddParam(string type, List<Define> defines, string operationName, IEnumerable<OpenApiParameter> parameters, List<Define.Method.Param> @params)
        {
            var paramName = $"{operationName}_{type}";

            defines.Add(new Define
            {
                Namespace = _namespace,
                Name = paramName,
                Properties = parameters.Select(s => new Define.Property
                {
                    Name = NullableWrap(s.Name, s.Required),
                    Type = TypeMapping(s.Schema)
                }).ToList()
            });

            @params.Add(new Define.Method.Param
            {
                Name = type,
                Type = paramName
            });
        }
        static string GetBody(IDictionary<string, OpenApiMediaType> content)
        {
            var schema = content.FirstOrDefault().Value?.Schema;
            if (schema == null) return "any";
            return TypeMapping(schema);
        }
        static string GetResponse(OpenApiResponses responses)
        {
            responses.TryGetValue("200", out var response);
            if (response == null) return "any";
            return TypeMapping(response.Content.FirstOrDefault().Value?.Schema);
        }
    }
}

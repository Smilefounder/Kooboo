using Kooboo.Sites.Scripting.KDefine;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Sites.OpenApi
{
    public class OpenApiDefineConventer : IDefineConventer
    {
        readonly OpenApiDocument _doc;
        readonly string _name;
        readonly string _namespace;

        public OpenApiDefineConventer(OpenApiDocument doc, string name, string @namespace)
        {
            _doc = doc;
            _name = name;
            _namespace = @namespace;
        }
        public Define[] Convent()
        {
            var defines = new List<Define>();

            var result = new Define
            {
                Namespace = _namespace,
                Name = _name,
                Methods = new List<Define.Method>()
            };

            if (_doc.Components?.Schemas != null) AddModels(defines, _doc.Components.Schemas);
            if (_doc.Paths != null) AddMethod(_doc.Paths, result.Methods);
            defines.Add(result);
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
        static void AddMethod(OpenApiPaths paths, List<Define.Method> methods)
        {
            foreach (var path in paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    methods.Add(new Define.Method
                    {
                        Name = Helpers.StandardPath(path.Key, operation.Key),
                        ReturnType = GetResponse(operation.Value.Responses),
                        Discription = operation.Value.Description,
                        Params = GetParams(operation.Value)
                    });
                }
            }
        }
        static List<Define.Method.Param> GetParams(OpenApiOperation operation)
        {
            var @params = new List<Define.Method.Param>();

            foreach (var item in operation.Parameters)
            {
                @params.Add(new Define.Method.Param
                {
                    Name = NullableWrap(item.Name, item.Schema),
                    Type = TypeMapping(item.Schema)
                });
            }

            if (operation.RequestBody != null)
            {
                @params.Add(new Define.Method.Param
                {
                    Name = "body",
                    Type = GetBody(operation.RequestBody.Content)
                });
            }

            return @params;
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

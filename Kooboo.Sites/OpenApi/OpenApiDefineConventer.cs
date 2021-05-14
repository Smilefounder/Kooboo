using Kooboo.Data.Models;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Scripting.KDefine;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System.Collections.Generic;

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

        public static string GetDefinesByWebSite(WebSite webSite)
        {
            var @namespace = "OpenApi";
            var result = new List<Define>();
            var openApis = webSite.SiteDb().OpenApi.All();
            var openApiProperties = new List<Define.Property>();

            foreach (var openApi in openApis)
            {
                var reader = new OpenApiStringReader();
                var doc = reader.Read(openApi.JsonData, out _);
                var name = Helpers.StandardName(openApi.Name);

                var openApiProperty = new Define.Property
                {
                    Name = name,
                    Type = $"{@namespace}.{name}",
                };

                var openApiDefine = new OpenApiDefineConventer(doc, name, @namespace);
                if (doc.Info != null) openApiDefine.AddInfoComment(openApiProperty);
                openApiProperties.Add(openApiProperty);
                var defines = new OpenApiDefineConventer(doc, name, @namespace).Convent();
                result.AddRange(defines);
            }

            KscriptAddOpenApi(result, openApiProperties, @namespace);
            return new DefineStringify(result.ToArray()).ToString();
        }

        static void KscriptAddOpenApi(List<Define> result, List<Define.Property> openApiProperties, string @namespace)
        {

            var name = "openApi";
            result.Add(new Define
            {
                Namespace = @namespace,
                Name = name,
                Properties = openApiProperties
            });

            result.Add(new Define
            {
                Namespace = "KScript",
                Name = "k",
                Properties = new List<Define.Property> {
                    new Define.Property{
                        Name=name,
                        Type=$"{@namespace}.{name}"
                    }
                }
            });
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

            if (_doc.Components != null)
            {
                //if (doc.Components.Schemas != null) AddModel(doc.Components.Schemas);
            }

            if (_doc.Paths != null) AddMethod(_doc.Paths, result.Methods);
            defines.Add(result);
            return defines.ToArray();
        }

        void AddInfoComment(Define.Property openApiProperty)
        {
            openApiProperty.Discription = $@"
Title: {_doc.Info.Title}
Contact: {_doc.Info.Contact}
Version: {_doc.Info.Version}
Description: {_doc.Info.Description}
";
        }

        static void AddMethod(OpenApiPaths paths, List<Define.Method> methods)
        {
            foreach (var path in paths)
            {
                foreach (var Operation in path.Value.Operations)
                {
                    methods.Add(new Define.Method
                    {
                        Name = Helpers.StandardPath(path.Key, Operation.Key)
                    });
                }
            }
        }
    }
}

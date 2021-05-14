using Kooboo.Data.Context;
using Kooboo.Data.Models;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Scripting.KDefine;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.OpenApi
{
    public static class OpenApiDefine
    {
        readonly static string _namespaceBase = "KScript.OpenApi";

        public static Define[] GetDefines(WebSite webSite)
        {
            var result = new List<Define>();

            result.Add(new Define
            {
                Namespace = "KScript",
                Name = "k",
                Properties = new List<Define.Property> {
                    new Define.Property{
                        Name="openApi",
                        Type="OpenApi.openApi"
                    }
                }
            });


            var openApiDefine = new Define
            {
                Namespace = _namespaceBase,
                Name = "openApi",
                Properties = new List<Define.Property>()
            };

            result.Add(openApiDefine);
            var openApis = webSite.SiteDb().OpenApi.All();

            foreach (var openApi in openApis)
            {
                var name = Helpers.StandardName(openApi.Name);

                openApiDefine.Properties.Add(new Define.Property
                {
                    Name = name,
                    Type = NamespaceWrap(name),
                });

                result.Add(OpenApiToDefine(openApi));
            }

            return result.ToArray();
        }

        static Define OpenApiToDefine(Models.OpenApi openApi)
        {
            var reader = new OpenApiStringReader();
            var doc = reader.Read(openApi.JsonData, out _);
            var result = new Define();
            var name = Helpers.StandardName(openApi.Name);
            result.Namespace = "KScript.OpenApi";
            result.Name = name;
            result.Methods = new List<Define.Method>();

            if (doc.Paths != null)
            {
                foreach (var path in doc.Paths)
                {
                    foreach (var Operation in path.Value.Operations)
                    {
                        var method = new Define.Method();
                        method.Name = Helpers.StandardPath(path.Key, Operation.Key);
                        result.Methods.Add(method);
                    }
                }
            }

            return result;
        }

        static string NamespaceWrap(string @namespace)
        {
            return $"{_namespaceBase}.{@namespace}";
        }
    }
}

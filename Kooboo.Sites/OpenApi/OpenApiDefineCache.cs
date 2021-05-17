using Kooboo.Data.Context;
using Kooboo.Data.Events;
using Kooboo.Data.Models;
using Kooboo.Events;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Scripting.KDefine;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.OpenApi
{
    public static class OpenApiDefineCache
    {
        static readonly ConcurrentDictionary<Guid, string> _cache = new ConcurrentDictionary<Guid, string>();

        public static string GetDefinesByWebSite(WebSite webSite)
        {
            return _cache.GetOrAdd(webSite.Id, _ => GetDefinesString(webSite));
        }

        public static void RemoveCache(WebSite webSite)
        {
            _cache.TryRemove(webSite.Id, out _);
        }

        static string GetDefinesString(WebSite webSite)
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

                if (doc.Info != null) AddInfoComment(openApiProperty, doc.Info);
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

        static void AddInfoComment(Define.Property openApiProperty, OpenApiInfo info)
        {
            openApiProperty.Discription = $@"
Title: {info.Title}
Contact: {info.Contact}
Version: {info.Version}
Description: {info.Description}
";
        }
    }

    class WebSiteChangeLessHandler : IHandler<WebSiteChange>
    {
        public void Handle(WebSiteChange theEvent, RenderContext context)
        {
            if (theEvent.ChangeType != ChangeType.Delete) return;
            OpenApiDefineCache.RemoveCache(theEvent.WebSite);
        }
    }
}

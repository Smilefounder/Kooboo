using Kooboo.Data.Context;
using Kooboo.Data.Events;
using Kooboo.Data.Models;
using Kooboo.Events;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Scripting.KDefine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kooboo.Sites.OpenApi
{
    public class Cache
    {
        static readonly ConcurrentDictionary<Guid, Cache> _cache = new ConcurrentDictionary<Guid, Cache>();
        readonly Lazy<Dictionary<string, Document>> _documents;
        readonly Lazy<string> _defineString;

        public Dictionary<string, Document> Documents => _documents.Value;
        public string DefineString => _defineString.Value;

        public Cache(WebSite webSite)
        {
            _documents = new Lazy<Dictionary<string, Document>>(() =>
            {
                var result = new Dictionary<string, Document>();
                var list = webSite.SiteDb().OpenApi.All();

                foreach (var item in list)
                {
                    var name = Helpers.StandardName(item.Name);
                    if (!result.ContainsKey(name)) result.Add(name, new Document(item));
                }

                return result;
            });

            _defineString = new Lazy<string>(GetDefinesString, true);
        }

        public static Cache Get(WebSite webSite)
        {
            return _cache.GetOrAdd(webSite.Id, _ => new Cache(webSite));
        }

        public static void Remove(WebSite webSite)
        {
            _cache.TryRemove(webSite.Id, out _);
        }

        string GetDefinesString()
        {
            var @namespace = "OpenApi";
            var defines = new List<Define>();
            var openApiProperties = new List<Define.Property>();

            foreach (var OpenApiDoc in Documents)
            {
                var name = OpenApiDoc.Key;
                var doc = OpenApiDoc.Value;

                openApiProperties.Add(new Define.Property
                {
                    Name = name,
                    Type = $"{@namespace}.{name}",
                    Discription = doc.Description
                });

                defines.AddRange(new DefineConventer(doc, name, @namespace).Convent());
            }

            defines.Add(new Define
            {
                Namespace = @namespace,
                Name = "openApi",
                Properties = openApiProperties
            });

            defines.Add(new Define
            {
                Namespace = "KScript",
                Name = "k",
                Properties = new List<Define.Property> {
                    new Define.Property{
                        Name="openApi",
                        Type=$"{@namespace}.openApi"
                    }
                }
            });

            var result= new DefineStringify(defines.ToArray()).ToString();
            return result;
        }
    }

    class WebSiteChangeOpenApiHandler : IHandler<WebSiteChange>
    {
        public void Handle(WebSiteChange theEvent, RenderContext context)
        {
            if (theEvent.ChangeType != ChangeType.Delete) return;
            Cache.Remove(theEvent.WebSite);
        }
    }
}

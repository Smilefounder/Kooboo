using Kooboo.Data.Context;
using Kooboo.Data.Events;
using Kooboo.Data.Models;
using Kooboo.Events;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Scripting.KDefine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kooboo.Sites.OpenApi
{
    public class Cache
    {
        static readonly ConcurrentDictionary<Guid, Cache> _cache = new ConcurrentDictionary<Guid, Cache>();
        readonly Lazy<Dictionary<string, Document>> _documents;
        readonly Lazy<string> _defineString;
        readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Dictionary<string, Document> Documents => _documents.Value;
        public string DefineString => _defineString.Value;

        public Cache(WebSite webSite)
        {
            _documents = new Lazy<Dictionary<string, Document>>(() => GetDocuments(webSite), true);
            _defineString = new Lazy<string>(GetDefinesString, true);
            StartCleanTask();
        }

        public static Cache Get(WebSite webSite)
        {
            return _cache.GetOrAdd(webSite.Id, _ => new Cache(webSite));
        }

        public static void Remove(WebSite webSite)
        {
            _cache.TryRemove(webSite.Id, out var cache);
            cache?.StopCleanTask();
        }

        public void StopCleanTask()
        {
            _cancellationTokenSource.Cancel();
        }

        Dictionary<string, Document> GetDocuments(WebSite webSite)
        {
            var result = new Dictionary<string, Document>();
            var list = webSite.SiteDb().OpenApi.All();

            foreach (var item in list)
            {
                var name = Helpers.StandardName(item.Name);
                if (!result.ContainsKey(name)) result.Add(name, new Document(item));
            }

            return result;
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

            var result = new DefineStringify(defines.ToArray()).ToString();
            return result;
        }

        void StartCleanTask()
        {
            var cleanTask = new Task(async () =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromDays(1), _cancellationTokenSource.Token);
                    }
                    catch (Exception)
                    {
                    }

                    if (_documents.IsValueCreated)
                    {
                        var operations = _documents.Value.SelectMany(doc => doc.Value.Operations);

                        foreach (var operation in operations)
                        {
                            var expireItems = operation.Value.Caches.Where(w => DateTime.UtcNow > w.Value.Item1).Select(s => s.Key);

                            foreach (var item in expireItems)
                            {
                                operation.Value.Caches.TryRemove(item, out _);
                            }
                        }
                    }
                }

            }, TaskCreationOptions.LongRunning);

            cleanTask.Start();
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

//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using Kooboo.Api;
using Kooboo.Sites.DataSources;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Models;
using Kooboo.Sites.Scripting;
using Kooboo.Web.Frontend.KScriptDefine;
using Kooboo.Web.ViewModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kooboo.Web.Api.Implementation
{
    public class KScriptApi : IApi
    {
        class Cache
        {
            public DateTime Expired { get; set; }
            public string Value { get; set; }
        }

        private readonly Lazy<string> _defineContent;


        private static readonly ConcurrentDictionary<string, Cache> _cdnLibCache = new ConcurrentDictionary<string, Cache>();

        static KScriptApi()
        {
            Task.Run(() =>
            {
                Thread.Sleep(new TimeSpan(3, 0, 0, 0));
                var expiredKeys = _cdnLibCache.Where(w => w.Value.Expired < DateTime.Now).Select(s => s.Key).ToArray();

                foreach (var key in expiredKeys)
                {
                    _cdnLibCache.TryRemove(key, out _);
                }
            });
        }

        public KScriptApi()
        {
            _defineContent = new Lazy<string>(() => new KScriptToTsDefineConventer().Convent(typeof(KScript.k)), true);
        }

        public string ModelName
        {
            get { return "KScript"; }
        }

        public bool RequireSite
        {
            get { return true; }
        }

        public bool RequireUser
        {
            get { return false; }
        }

        public string GetDefine()
        {
            return _defineContent.Value;
        }

        [Kooboo.Attributes.RequireParameters("name")]
        public string GetLib(string name, ApiCall call)
        {
            var script = call.WebSite.SiteDb().Scripts.GetByNameOrId($"{name}.js");
            if (script == null) call.WebSite.SiteDb().Scripts.GetByNameOrId($"{name}.min.js");
            if (script != null) return script.Body;

            if (_cdnLibCache.TryGetValue(name, out var cache))
            {
                cache.Expired = DateTime.Now.AddMonths(1);
                return cache.Value;
            }
            else
            {
                var client = new System.Net.WebClient();
                var result = client.DownloadString($"https://cdn.jsdelivr.net/npm/{name}");
                _cdnLibCache.TryAdd(name, new Cache { Expired = DateTime.Now.AddMonths(1), Value = result });
                return result;
            }
        }
    }
}

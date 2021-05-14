using Kooboo.Data.Models;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.OpenApi;
using KScript;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Scripting.KDefine
{
    public static class DefineManager
    {
        static readonly Lazy<TypeDefine> _kDefine = new Lazy<TypeDefine>(() => new TypeDefine(typeof(k)), true);
        static readonly Lazy<string> _kDefineString = new Lazy<string>(() => new DefineStringify(_kDefine.Value.Defines).ToString(), true);
        static readonly ConcurrentDictionary<Guid, string> _openApiDefine = new ConcurrentDictionary<Guid, string>();

        public static string GetDefine(WebSite webSite)
        {
            var sb = new StringBuilder();
            sb.AppendLine(_kDefineString.Value);
            sb.AppendLine(GetOpenApiDefine(webSite));
            sb.AppendLine($"declare const k: {_kDefine.Value.TypeName};");
            return sb.ToString();
        }

        static string GetOpenApiDefine(WebSite webSite)
        {
            return _openApiDefine.GetOrAdd(webSite.Id, _ =>
            {
                var defines = OpenApiDefine.GetDefines(webSite);
                return new DefineStringify(defines).ToString();
            });
        }
    }
}

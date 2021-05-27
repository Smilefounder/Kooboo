using Jint.Native;
using Jint.Native.Function;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Sites.OpenApi
{
    public class Executer : FunctionInstance
    {
        readonly string _openApiName;
        readonly RenderContext _renderContext;

        public string PathName { get; }

        public Executer(string openApiName, string pathName, RenderContext renderContext)
            : base(Manager.GetJsEngine(renderContext), null, null, false)
        {
            _openApiName = openApiName;
            PathName = pathName;
            _renderContext = renderContext;
        }

        public override JsValue Call(JsValue _, JsValue[] arguments)
        {
            var webSiteCache = Cache.Get(_renderContext.WebSite);
            webSiteCache.Documents.TryGetValue(_openApiName, out var doc);
            if (doc == null) throw new Exception($"Can not found openApi {_openApiName}");
            doc.Operations.TryGetValue(PathName, out var operation);
            if (operation == null) throw new Exception($"Can not found openApi operation {PathName}");
            var result = operation.Send(_renderContext, arguments.Select(s => s.ToObject()));
            return JsValue.FromObject(Engine, result);
        }
    }
}

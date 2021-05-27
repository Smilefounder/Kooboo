using Kooboo.Data.Context;
using System;

namespace Kooboo.Sites.OpenApi
{
    public class KOpenApi
    {
        readonly RenderContext _context;

        public KOpenApi(RenderContext context)
        {
            _context = context;
        }

        public object this[string key] => Get(key);

        public object Get(string name) => new _(name, _context);

        class _
        {
            readonly string _openApiName;
            readonly RenderContext _context;

            public _(string openApiName, RenderContext context)
            {
                _openApiName = openApiName;
                _context = context;
            }

            public object this[string key] => Get(key);

            public object Get(string name) => new Executer(_openApiName, name, _context);
        }
    }
}

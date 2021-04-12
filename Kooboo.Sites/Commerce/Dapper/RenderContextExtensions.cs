using Kooboo.Data;
using Kooboo.Data.Context;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace Kooboo.Sites.Commerce
{
    public static class RenderContextExtensions
    {
        static readonly ConcurrentDictionary<string, string> _dic = new ConcurrentDictionary<string, string>();

        public static IDbConnection CreateCommerceDbConnection(this RenderContext context)
        {
            var connectionString = _dic.GetOrAdd($"{context.WebSite.OrganizationId}{context.WebSite.Id}", s =>
             {
                 var path = Path.Combine(AppSettings.GetFileIORoot(context.WebSite), "commerce.db");
                 return $"Data source = '{path}';Version=3;BinaryGUID=False;foreign keys=true";
             });

            return new SQLiteConnection(connectionString);
        }
    }
}

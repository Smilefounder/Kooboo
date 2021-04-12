using Kooboo.Data;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Migration;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Kooboo.Sites.Commerce
{
    public class ServiceBase
    {
        protected RenderContext Context { get; set; }
        protected IDbConnection DbConnection => Context.CreateCommerceDbConnection();

        public ServiceBase(RenderContext context)
        {
            Context = context;
            if(!Migrator.IsMigrated(context.WebSite.Id)) Migrator.TryMigrate(context.WebSite.Id, DbConnection);
        }

    }
}

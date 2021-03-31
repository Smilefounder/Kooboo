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
        private readonly string _connectionString;
        protected RenderContext Context { get; set; }
        protected IDbConnection DbConnection => new SQLiteConnection(_connectionString);

        public ServiceBase(RenderContext context)
        {
            Context = context;
            var path = Path.Combine(AppSettings.GetFileIORoot(context.WebSite), "commerce.db");
            _connectionString = $"Data source = '{path}';Version=3;BinaryGUID=False;foreign keys=true";
            if(!Migrator.IsMigrated(context.WebSite.Id)) Migrator.TryMigrate(context.WebSite.Id, DbConnection);
        }

    }
}

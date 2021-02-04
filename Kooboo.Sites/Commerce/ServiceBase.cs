using Dapper;
using Kooboo.Data;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Dapper;
using Kooboo.Sites.Commerce.Migration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace Kooboo.Sites.Commerce
{
    public class ServiceBase
    {
        private readonly string _connectionString;
        protected RenderContext Context { get; set; }
        protected IDbConnection DbConnection => new SQLiteConnection(_connectionString);

        static ServiceBase()
        {
            SqlMapper.AddTypeHandler(typeof(Guid), new GuidHandler());
        }

        public ServiceBase(RenderContext context)
        {
            Context = context;
            var path = Path.Combine(AppSettings.GetFileIORoot(context.WebSite), "commerce.db");
            _connectionString = $"Data source = '{path}';Version=3;BinaryGUID=False";

            using (var con = DbConnection)
            {
                con.Open();
                Migrator.TryMigrate(context.WebSite.Id, con, true);
            }
        }

    }
}

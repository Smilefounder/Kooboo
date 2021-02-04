using Dapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Migration
{
    public static class Migrator
    {
        readonly static object _locker = new object();
        readonly static List<Guid> _migratedSites = new List<Guid>();
        readonly static List<IMigration> _migrationRecords = Lib.IOC.Service.GetInstances<IMigration>();


        public static void TryMigrate(Guid siteId, IDbConnection connection, bool rebuild = false)
        {
            bool migrated = false;

            lock (_locker)
            {
                migrated = _migratedSites.Contains(siteId);
                if (!migrated) _migratedSites.Add(siteId);
            }

            if (!migrated)
            {
                if (rebuild)
                {
                    foreach (var item in connection.Query<string>("select name from sqlite_master where type='table'"))
                    {
                        connection.Execute("drop table " + item);
                    }
                }

                Migrate(connection);
            }
        }

        public static void Migrate(IDbConnection connection)
        {
            connection.Execute("create table if not exists _migration( ver integer not null);");
            var lastVersion = connection.QueryFirstOrDefault<int?>("select max(ver) from _migration") ?? 0;
            var appends = _migrationRecords.Where(w => w.Version > lastVersion).OrderBy(o => o.Version);

            foreach (var item in appends)
            {
                var tran = connection.BeginTransaction();
                item.Migrate(connection);
                connection.Execute($"INSERT INTO _migration (ver) VALUES({item.Version});");
                tran.Commit();
            }
        }
    }
}

using Kooboo.Data;
using Kooboo.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using static Kooboo.Constants;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.Linq;

namespace Kooboo.Mail.Repositories.Sqlite
{
    public class DbUpdate
    {
        static string ResourceNamespace = "Kooboo.Mail.Repositories.Sqlite";

        private OneUpdate[] _updates;

        public DbUpdate()
        {
            Init();
        }


        public void Execute()
        {
            foreach (var each in GlobalDb.WebSites.AllSites.Values)
            {
                ExecuteSite(each);
            }
        }

        private void ExecuteSite(WebSite site)
        {
            Console.WriteLine($"Update {site.Name} DB");
            var path = Path.Combine(AppSettings.GetFileIORoot(site), "sqlite.db");
            using (var con = new SQLiteConnection($"Data source='{path}';"))
            {
                con.Open();
                ExecuteDb(con);
            }
        }

        private void ExecuteDb(SQLiteConnection con)
        {
            var existed = GetExistedUpdates(con);
            var newUpdates = _updates.Where(o => !existed.Contains(o.Name, StringComparer.OrdinalIgnoreCase));
            foreach (var each in newUpdates)
            {
                ExecuteOneUpdate(con, each);
            }
        }

        private void ExecuteOneUpdate(SQLiteConnection con, OneUpdate update)
        {
            var sql = ReadText($"{ResourceNamespace}.scripts.{update.Name}.sql");

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = $"insert into __DbHistory (Id) values ('{update.Name}')";
                cmd.ExecuteNonQuery();
            }
        }

        private List<string> GetExistedUpdates(SQLiteConnection con)
        {
            if (!ExistDbHistory(con))
                return new List<string>();

            var list = new List<string>();
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = "select * from __DbHistory";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var name = reader.GetString(0);
                    list.Add(name);
                }
            }

            return list;
        }

        private bool ExistDbHistory(SQLiteConnection con)
        {
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = $"SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = '__DbHistory'";
                return ((long)cmd.ExecuteScalar()) > 0;
            }
        }

        private void Init()
        {
            var json = ReadText($"{ResourceNamespace}.history.json");
            _updates = JsonConvert.DeserializeObject<OneUpdate[]>(json);
        }

        private string ReadText(string resourceName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public class OneUpdate
        {
            public string Name { get; set; }

            public string Operator { get; set; }

            public List<string> EFNames { get; set; }
        }
    }
}

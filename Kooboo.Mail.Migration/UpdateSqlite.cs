using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Kooboo.Mail
{
    class UpdateSqlite
    {
        static readonly string _dir = Directory.GetCurrentDirectory();

        static readonly string _projectPath = Path.Combine(_dir, @"..\Kooboo.Mail");
        static readonly string _historyRelativePath = @"Repositories\Sqlite";
        static readonly string _historyPath = Path.Combine(_projectPath, _historyRelativePath, "history.json");

        public void Execute()
        {
            // Get history
            var history = GetHistory();
            var historyNames = history.SelectMany(o => o.EfNames).ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Get all ef names
            var allEfNames = Directory.GetFiles(Path.Combine(_dir, "Migrations"), "*.cs")
                .Select(o => Path.GetFileNameWithoutExtension(o))
                .Where(o => o.IndexOf('_') > 0 && !o.EndsWith(".Designer"))
                .OrderBy(o => o)
                .ToArray();

            var newNames = new List<string>();
            var sql = new StringBuilder();

            // Segment new names into batches
            var batch = new List<string>();
            var lastEfName = "0";
            for (int i = 0; i < allEfNames.Length; i++)
            {
                var efName = allEfNames[i];
                if (historyNames.Contains(efName))
                {
                   if (batch.Count > 0)
                   {
                        GenerateSql(lastEfName, batch, sql);
                        newNames.AddRange(batch);
                        batch.Clear();
                    }
                    lastEfName = efName;
                }
                else
                {
                    batch.Add(efName);
                }
            }

            if (batch.Count > 0)
            {
                GenerateSql(lastEfName, batch, sql);
                newNames.AddRange(batch);
            }

            if (newNames.Count > 0)
            {
                // Write SQL to scripts folder
                var updateName = DateTime.Now.ToString("yyyyMMddHHmmss");
                File.WriteAllText(Path.Combine(_projectPath, $"{_historyRelativePath}\\scripts\\{updateName}.sql"), sql.ToString());

                // Add to history.json
                history.Add(new OneUpdate
                {
                    Name = updateName,
                    Operator = "+",
                    EfNames = newNames
                });
                File.WriteAllText(_historyPath, JsonConvert.SerializeObject(history, Formatting.Indented));

                // Declare new SQL as embeded resource
                var builder = new StringBuilder();
                var path = Path.Combine(_projectPath, "Kooboo.Mail.csproj");
                var text = File.ReadAllText(path);
                using (var reader = new StringReader(text))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        builder.AppendLine(line);
                        if (line.IndexOf($"<EmbeddedResource Include=\"{_historyRelativePath}\\history.json\" />") >= 0)
                        {
                            var embed = $"<EmbeddedResource Include=\"{_historyRelativePath}\\scripts\\{updateName}.sql\" />";
                            Console.WriteLine(embed);
                            builder.AppendLine("\t\t" + embed);
                            builder.Append(reader.ReadToEnd());
                            break;
                        }

                        line = reader.ReadLine();
                    }
                }
                File.WriteAllText(path, builder.ToString());
            }
        }

        private void GenerateSql(string lastEfName, List<string> batch, StringBuilder sql)
        {
            var appName = "dotnet";
            var args = $"ef migrations script {lastEfName} {batch.Last()} --output batch.sql --no-build";
            Console.WriteLine(appName + " " + args);

            using (var process = Process.Start(appName, args))
            {
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    var batchSql = File.ReadAllText(Path.Combine(_dir, "batch.sql"));
                    sql.AppendLine(batchSql);
                }
                else
                {
                    throw new Exception("ef script failed");
                }
            }
        }

        private List<OneUpdate> GetHistory()
        {
            var json = File.ReadAllText(_historyPath);
            return JsonConvert.DeserializeObject<List<OneUpdate>>(json);
        }

        public class OneUpdate
        {
            public string Name { get; set; }

            public string Operator { get; set; }

            public List<string> EfNames { get; set; }
        }
    }
}

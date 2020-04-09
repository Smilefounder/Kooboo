using System;
using System.IO;

namespace Kooboo.Web.WpImport
{
    public static class ImportLogger
    {
        public static string logPath = Path.Combine("wp", "log.txt");

        static ImportLogger()
        {
            if (!Directory.Exists("wp")) Directory.CreateDirectory("wp");
        }

        public static void Write(string text)
        {
            System.IO.File.AppendAllText(logPath, $"{text}{Environment.NewLine}");
        }
    }
}

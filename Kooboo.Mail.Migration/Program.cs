using System;
using System.IO;

namespace Kooboo.Mail
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "sqlite")
            {
                new UpdateSqlite().Execute();
            }
        }
    }
}

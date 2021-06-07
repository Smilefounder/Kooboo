//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using Kooboo.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Mail
{
    public class EmailEnvironment
    {
        static EmailEnvironment()
        {
            FQDN = AppSettingsUtility.Get("fqdn", "kooboo.com");
            AppName = AppSettingsUtility.Get("appName", "Kooboo");
            MTA = AppSettingsUtility.Get("mta", "127.0.0.1");
        }

        public static string AppName { get; set; }

        public static string FQDN { get; set; }

        public static string MTA { get; set; }
    }
}

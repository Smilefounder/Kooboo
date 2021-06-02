using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Data.Server
{
   public static  class WebServerFactory
    { 
        public static IWebServer Create(int port, List<IKoobooMiddleWare> MiddleWares, bool forceSSL=false)
        {
            KestrelWebServer server = new KestrelWebServer(port, MiddleWares, forceSSL);
            server.SetMiddleWares(MiddleWares);
            return server; 
        }
    }
}

using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Kooboo.Sites.OpenApi.Senders
{
    public class Json : HttpSender
    {
        protected override string ContentType => Operation.DefaultContentType;

        protected override byte[] SerializeBody(object body, HttpWebRequest request)
        {
            var json = JsonHelper.Serialize(body);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}

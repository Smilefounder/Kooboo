using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Kooboo.Sites.OpenApi.Senders
{
    public class FormUrlencoded : HttpSender
    {
        protected override string ContentType => "application/x-www-form-urlencoded";

        protected override byte[] SerializeBody(object body, HttpWebRequest request)
        {
            var data = body as IDictionary<string, object>;
            var str = string.Join("&", data.Select(s => $"{s.Key}={s.Value}"));
            return Encoding.UTF8.GetBytes(str);
        }
    }
}

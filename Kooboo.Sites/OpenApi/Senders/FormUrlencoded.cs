using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.OpenApi.Senders
{
    public class FormUrlencoded : HttpSender
    {
        protected override string ContentType => "application/x-www-form-urlencoded";

        protected override string SerializeBody(object body)
        {
            var data = body as IDictionary<string, object>;
            return string.Join("&", data.Select(s => $"{s.Key}={s.Value}"));
        }
    }
}

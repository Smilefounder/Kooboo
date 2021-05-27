using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.OpenApi.Senders
{
    public class Json : HttpSender
    {
        protected override string ContentType => Operation.DefaultContentType;

        protected override string SerializeBody(object body)
        {
            return JsonHelper.Serialize(body);
        }
    }
}

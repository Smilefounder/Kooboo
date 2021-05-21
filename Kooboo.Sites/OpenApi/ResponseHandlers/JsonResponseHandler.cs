using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.OpenApi.ResponseHandlers
{
    public class JsonResponseHandler : ResponseHandler
    {
        protected override string ContentType => Executer.DefaultContentType;

        public override object Handler(string data)
        {
            return JsonHelper.Deserialize(data);
        }
    }
}

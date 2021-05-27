using Jint.Native.Json;
using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.OpenApi.ResponseHandlers
{
    public class JsonResponse : ResponseHandler
    {
        protected override string ContentType => Operation.DefaultContentType;
        static readonly JsonParser _jsonParser = new JsonParser(new Jint.Engine());

        public override object Handler(string data)
        {
            return _jsonParser.Parse(data).ToObject();
        }
    }
}

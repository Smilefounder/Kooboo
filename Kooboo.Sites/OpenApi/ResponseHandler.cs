using Kooboo.Sites.OpenApi.ResponseHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.OpenApi
{
    public abstract class ResponseHandler
    {
        protected abstract string ContentType { get; }

        static readonly List<ResponseHandler> _responseHandlers = Lib.IOC.Service.GetInstances<ResponseHandler>();

        public abstract object Handler(string data);

        public static ResponseHandler Get(string contentType)
        {
            var handler = _responseHandlers.FirstOrDefault(f => contentType.Contains(f.ContentType));
            if (handler == null) handler = _responseHandlers.First(f => f is JsonResponseHandler);
            return handler;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.OpenApi.ResponseHandlers
{
    public class FormUrlencoded : ResponseHandler
    {
        protected override string ContentType => "application/x-www-form-urlencoded";

        public override object Handler(string data)
        {
            return data.Split('&').Select(s => s.Split('=')).ToDictionary(s => s.First(), s => (object)s.Last());
        }
    }
}

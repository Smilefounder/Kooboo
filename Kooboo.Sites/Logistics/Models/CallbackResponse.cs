using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Models
{
    public class CallbackResponse
    {
        public string ContentType { get; set; }

        public string Content { get; set; }

        public int StatusCode { get; set; } = 200;
    }
}

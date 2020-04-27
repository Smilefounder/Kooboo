using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.deppon.Model
{
    public class PostageRequest
    {
        public string DestCity { get; set; }
        public string DestProvince { get; set; }
        public string DestDistrict { get; set; }
        public string OriginalCity { get; set; }
        public string OriginalProvince { get; set; }
    }
}

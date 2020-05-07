using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.deppon.Model
{
    public class PostageResponse
    {
        public string Reason { get; set; }

        public List<ResponseParam> ResponseParam { get; set; }

        public string UniquerRequestNumber { get; set; }

        public string Result { get; set; }
    }

    public class ResponseParam
    {
        public string ProductName { get; set; }

        public string Totalfee { get; set; }
    }
}

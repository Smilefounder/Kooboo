using System.Collections.Specialized;

namespace Kooboo.Sites.Logistics.Methods.zop.Models
{
    public class ZOPRequest
    {
        public string url { set; get; }
        public NameValueCollection requestParams { get; }
        public int timeout = 2000;

        public ZOPRequest()
        {
            this.requestParams = new NameValueCollection();
        }

        public ZOPRequest addParam(string k, string v)
        {
            requestParams.Add(k, v);
            return this;
        }

    }
}

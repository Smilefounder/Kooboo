using System.Collections.Specialized;

namespace Kooboo.Sites.Logistics.Methods.zto.Models
{
    public class ZopRequest
    {
        public string url { set; get; }
        public NameValueCollection requestParams { get; }
        public int timeout = 2000;

        public ZopRequest()
        {
            this.requestParams = new NameValueCollection();
        }

        public ZopRequest addParam(string k, string v)
        {
            requestParams.Add(k, v);
            return this;
        }

    }
}

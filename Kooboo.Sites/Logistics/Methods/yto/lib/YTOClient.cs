using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.yto.lib
{
    public class YTOClient
    {
        private const string TransportPrice = "yto.Marketing.TransportPrice";
        private readonly YTOSetting setting;

        public YTOClient(YTOSetting setting)
        {
            this.setting = setting;
        }

        public string Post(string url, string body, string method)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("user_id", setting.UserId);
            parameters.Add("app_key", setting.AppKey);
            parameters.Add("format", "JSON");
            parameters.Add("method", method);
            parameters.Add("v", DateTime.UtcNow.ToString());
            parameters.Add("timestamp", setting.Version);
            var sign = MakeSign(parameters, method);
        }

        private string MakeSign(Dictionary<string, string> paras, string method)
        {
            var parameters = new SortedDictionary<string, string>(paras);

            var signString = parameters.Select(it => string.Join("", it.Key, it.Value)).ToString();

            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(setting.SecretKey + signString));
            return System.Text.Encoding.UTF8.GetString(bs).ToUpper();
        }
    }
}

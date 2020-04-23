using System;
using System.Security.Cryptography;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.best.lib
{
    public class SignUtil
    {
        public static string MakeSign(string origin)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(origin);
            byte[] targetData = md5.ComputeHash(fromData);
            return Convert.ToBase64String(targetData);
        }

        public static string MakeMd5Sign(string origin)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] targetData = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(origin));
            StringBuilder sb = new StringBuilder("");
            foreach (byte b in targetData)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
    }
}

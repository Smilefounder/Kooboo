using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.sf.lib
{
    public class VerifyCodeUtil
    {
        public static string md5EncryptAndBase64(string str)
        {
            return VerifyCodeUtil.encodeBase64(VerifyCodeUtil.md5Encrypt(str));
        }

        private static byte[] md5Encrypt(string encryptStr)
        {
            try
            {
                return new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(encryptStr));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
        }

        private static string encodeBase64(byte[] b)
        {
            return Convert.ToBase64String(b);
        }
    }
}

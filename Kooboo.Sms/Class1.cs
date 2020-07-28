using System;
using System.Linq;

using TencentCloud.Common;
using TencentCloud.Sms.V20190711;
using TencentCloud.Sms.V20190711.Models;

namespace Kooboo.Sms
{
    public class Test
    {
        public void Run()
        {
            var secretId = "AKIDiCPKtBxuLVOQBthwg9LYhZ8Qk0zgLWtz";
            var secretKey = "xd17vRfcHejPpwmJZL3Udz3pJXS5QTFm";

            Credential cred = new Credential
            {
                SecretId = secretId,
                SecretKey = secretKey
            };

            var client = new SmsClient(cred, "ap-guangzhou");
            var req = new SendSmsRequest();

            req.SmsSdkAppid = "1400365470";
            /* 短信签名内容: 使用 UTF-8 编码，必须填写已审核通过的签名，可登录 [短信控制台] 查看签名信息 */
            req.Sign = "亚尔迪";
            req.SenderId = "";
            req.PhoneNumberSet = new[] { $"+86{15359241007}" };
            /* 模板 ID: 必须填写已审核通过的模板 ID，可登录 [短信控制台] 查看模板 ID */
            req.TemplateID = "602974";
            /* 模板参数: 若无模板参数，则设置为空*/
            req.TemplateParamSet = new string[] { "123456", "5" };


            // 通过 client 对象调用 SendSms 方法发起请求，注意请求方法名与请求对象是对应的
            // 返回的 resp 是一个 SendSmsResponse 类的实例，与请求对象对应
            var resp = client.SendSms(req).Result;
            var first = resp.SendStatusSet.FirstOrDefault();
            var result = first != null && first.Code.Equals("Ok", StringComparison.OrdinalIgnoreCase);
        }
    }
}

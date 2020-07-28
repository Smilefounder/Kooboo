using System;
using System.Linq;

using TencentCloud.Common;
using TencentCloud.Sms.V20190711;
using TencentCloud.Sms.V20190711.Models;

namespace Kooboo.Sms.TencentCloud
{
    public class Sample
    {
        public void Run()
        {
            var secretId = "AKIDiCPKtBxuLVOQBthwg9LYhZ8Qk0zgLWtz";
            var secretKey = "xd17vRfcHejPpwmJZL3Udz3pJXS5QTFm";
            var appId = "1400365470";
            var sign = "亚尔迪";
            var regionId = "ap-guangzhou";

            var templateId = "602974";
            var phoneNumber = "+86" + "15359241007";

            var client = new SmsClient(new Credential
            {
                SecretId = secretId,
                SecretKey = secretKey
            }, regionId);
            var req = new SendSmsRequest();

            req.SmsSdkAppid = appId;
            /* 短信签名内容: 使用 UTF-8 编码，必须填写已审核通过的签名，可登录 [短信控制台] 查看签名信息 */
            req.Sign = sign;
            req.SenderId = string.Empty;

            req.TemplateID = templateId;
            req.PhoneNumberSet = new[] { phoneNumber };
            req.TemplateParamSet = new string[] { "123456", "5" };

            var resp = client.SendSms(req).Result;
            var first = resp.SendStatusSet.FirstOrDefault();
            var result = first != null && first.Code.Equals("Ok", StringComparison.OrdinalIgnoreCase);
        }
    }
}

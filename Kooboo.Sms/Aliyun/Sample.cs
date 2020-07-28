using System;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Newtonsoft.Json;

namespace Kooboo.Sms.Aliyun
{
    public class Sample
    {
        public void Run()
        {
            var accessId = "LTAIGIB79mwpXARD";
            var accessSecret = "lmI8b1KMejcLiUfWSj8J3nOp9QdBQR";
            var regionId = "cn-hangzhou";
            var domain = "dysmsapi.aliyuncs.com";
            var product = "Dysmsapi";

            var signName = "Kooboo";
            var templateCode = "SMS_145295286";
            var phoneNumber = "+86" + "15359241007";
            var message = "Kooboo短信集成测试";

            var profile = DefaultProfile.GetProfile(regionId, accessId, accessSecret);
            profile.AddEndpoint(regionId, regionId, product, domain);

            var acsClient = new DefaultAcsClient(profile);
            var request = new SendSmsRequest();
            try
            {
                request.PhoneNumbers = phoneNumber;
                request.SignName = signName;
                request.TemplateCode = templateCode;

                request.TemplateParam = JsonConvert.SerializeObject(new
                {
                    message
                });

                var sendSmsResponse = acsClient.GetAcsResponse(request);
            }
            catch (ServerException e)
            {
                System.Console.WriteLine("sms ServerException");
            }
            catch (ClientException e)
            {
                System.Console.WriteLine("sms ClientException");
            }
        }
    }
}

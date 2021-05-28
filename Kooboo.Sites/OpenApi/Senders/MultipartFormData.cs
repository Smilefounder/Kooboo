using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Kooboo.Sites.OpenApi.Senders
{
    public class MultipartFormData : HttpSender
    {
        protected override string ContentType => "multipart/form-data";

        protected override byte[] SerializeBody(object body, HttpWebRequest request)
        {
            var form = new MultipartFormDataContent();
            request.KeepAlive = true;
            request.ContentType = form.Headers.ContentType.ToString();

            if (body is KScript.UploadFile)
            {
                var file = body as KScript.UploadFile;
                form.Add(new ByteArrayContent(file.Bytes), "file", file.FileName);
            }

            if (body is IDictionary<string, object>)
            {
                var dic = body as IDictionary<string, object>;

                foreach (var item in dic)
                {
                    if (item.Value is KScript.UploadFile)
                    {
                        var file = item.Value as KScript.UploadFile;
                        form.Add(new ByteArrayContent(file.Bytes), item.Key, file.FileName);
                    }
                    else
                    {
                        form.Add(new StringContent(item.Value.ToString()), item.Key);
                    }
                }
            }

            return form.ReadAsByteArrayAsync().Result;
        }
    }
}

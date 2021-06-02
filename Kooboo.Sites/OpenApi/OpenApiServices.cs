using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kooboo.Sites.OpenApi
{
    public class OpenApiServices
    {
        class Template
        {
            public string Name { get; set; }
            public string Code { get; set; }
            public string Document { get; set; }
        }

        readonly RenderContext context;

        public OpenApiServices(RenderContext context)
        {
            this.context = context;
        }

        public Guid Save(Models.OpenApi openApi)
        {
            if (openApi.Type == "url") openApi.JsonData = HttpClientHelper.Client.GetStringAsync(openApi.Url).Result;
            if (openApi.Type == "template") GetTemplate(openApi);
            openApi.JsonData = StandardizationDocument(openApi);
            if (openApi.Securities == null) openApi.Securities = new Dictionary<string, Models.OpenApi.AuthorizeData>();
            context.WebSite.SiteDb().OpenApi.AddOrUpdate(openApi);
            return openApi.Id;
        }

        private static string StandardizationDocument(Models.OpenApi openApi)
        {
            OpenApiDocument doc;

            if (string.IsNullOrWhiteSpace(openApi.JsonData))
            {
                doc = new OpenApiDocument();
            }
            else
            {
                doc = new OpenApiStringReader().Read(openApi.JsonData, out var _);
            }

            using (var textWriter = new StringWriter())
            {
                var jsonWriter = new OpenApiJsonWriter(textWriter);
                doc.SerializeAsV3(jsonWriter);
                textWriter.Flush();
                return textWriter.ToString();
            }
        }

        private void GetTemplate(Models.OpenApi openApi)
        {
            if (string.IsNullOrWhiteSpace(openApi.Url)) throw new Exception("you should select a template");
            var result = HttpClientHelper.Client.GetStringAsync(openApi.Url).Result;
            var tempalte = JsonHelper.Deserialize<Template>(result);

            if (!string.IsNullOrWhiteSpace(tempalte.Code))
            {
                openApi.CustomAuthorization = $"{tempalte.Name}_custom_authorization";
                var code = context.WebSite.SiteDb().Code.GetByNameOrId(openApi.CustomAuthorization);

                if (code == null) context.WebSite.SiteDb().Code.AddOrUpdate(new Models.Code
                {
                    Name = openApi.CustomAuthorization,
                    CodeType = Models.CodeType.Authorization,
                    Body = tempalte.Code
                });
            }
            openApi.JsonData = tempalte.Document;
        }
    }
}

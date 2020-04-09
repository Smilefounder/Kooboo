using Kooboo.Api;
using Kooboo.Data.Context;
using Kooboo.Data.Models;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Extensions;
using Kooboo.Web.Api;
using Kooboo.Web.Api.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Kooboo.Web.WpImport
{
    public static class WpImporter
    {
        public static void Import(ApiCall call)
        {
            var sitePaths = Directory.GetDirectories("wp");
            foreach (var sitePath in sitePaths)
            {
                ImportSite(call, sitePath);
            }
        }

        private static void ImportSite(ApiCall call, string sitePath)
        {
            var siteName = Path.GetFileName(sitePath);
            var newsite = Kooboo.Sites.Service.WebSiteService.AddNewSite(call.Context.User.CurrentOrgId, siteName, $"{siteName}.localkooboo.com", call.Context.User.Id);
            ImportScripts(sitePath, newsite, call.Context);
            ImportStyles(sitePath, newsite, call.Context);
            ImportImages(sitePath, newsite, call.Context);
            ImportFiles(sitePath, newsite, call.Context);

        }

        private static void ImportFiles(string sitePath, WebSite newsite, RenderContext context)
        {
            var files = Directory.GetFiles(Path.Combine(sitePath, "static"));
            foreach (var image in files)
            {
                var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(File.ReadAllBytes(image)), "'file_0'", $"'{Path.GetFileName(image)}'");
                content.Add(new StringContent("static"), "'Folder'");
                context.Request.PostData = content.ReadAsByteArrayAsync().Result;

                var call = new ApiCall
                {
                    ObjectId = default(Guid),
                    WebSite = newsite,
                    Context = context
                };

                new UploadApi().File(call);
            }
        }

        private static void ImportImages(string sitePath, WebSite newsite, RenderContext context)
        {
            var images = Directory.GetFiles(Path.Combine(sitePath, "image"));
            foreach (var image in images)
            {
                var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(File.ReadAllBytes(image)), "'file_0'", $"'{Path.GetFileName(image)}'");
                content.Add(new StringContent("image"), "'Folder'");
                context.Request.PostData = content.ReadAsByteArrayAsync().Result;

                var call = new ApiCall
                {
                    ObjectId = default(Guid),
                    WebSite = newsite,
                    Context = context
                };

                new UploadApi().Image(call);
            }
        }

        private static void ImportStyles(string sitePath, WebSite newsite, RenderContext context)
        {
            var styles = Directory.GetFiles(Path.Combine(sitePath, "css"));

            foreach (var style in styles)
            {
                context.Request.Body = JsonHelper.Serialize(new
                {
                    body = File.ReadAllText(style),
                    name = style.Substring(sitePath.Length)
                });

                var call = new ApiCall
                {
                    ObjectId = default(Guid),
                    WebSite = newsite,
                    Context = context
                };

                new StyleApi().Update(call);
            }
        }

        private static void ImportScripts(string sitePath, Data.Models.WebSite newsite, RenderContext context)
        {
            var scripts = Directory.GetFiles(Path.Combine(sitePath, "js"));

            foreach (var script in scripts)
            {
                context.Request.Body = JsonHelper.Serialize(new
                {
                    body = File.ReadAllText(script),
                    name = script.Substring(sitePath.Length)
                });

                var call = new ApiCall
                {
                    ObjectId = default(Guid),
                    WebSite = newsite,
                    Context = context
                };

                new ScriptApi().Update(call);
            }
        }
    }
}

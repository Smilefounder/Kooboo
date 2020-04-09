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
            if (File.Exists(ImportLogger.logPath)) File.Delete(ImportLogger.logPath);
            ImportLogger.Write(DateTime.Now.ToString());
            var sitePaths = Directory.GetDirectories("wp");
            foreach (var sitePath in sitePaths)
            {
                ImportLogger.Write($"----------开始导入网站 {sitePath}");
                ImportSite(call, sitePath);
                ImportLogger.Write($"----------结束导入网站 {sitePath}");
            }
        }

        private static void ImportSite(ApiCall call, string sitePath)
        {
            WebSite website = null;

            try
            {
                var siteName = Path.GetFileName(sitePath);
                website = Kooboo.Sites.Service.WebSiteService.AddNewSite(call.Context.User.CurrentOrgId, siteName, $"{siteName}.localkooboo.com", call.Context.User.Id);
                ImportScripts(sitePath, website, call.Context);
                ImportStyles(sitePath, website, call.Context);
                ImportImages(sitePath, website, call.Context);
                ImportFiles(sitePath, website, call.Context);
                //TODO ImportData(sitePath, newsite, call.Context);
                ImportHtmlblock(sitePath, website, call.Context);
            }
            catch (Exception ex)
            {
                ImportLogger.Write($"----------导入失败 {ex}");
                if (website != null) Kooboo.Sites.Service.WebSiteService.Delete(website.Id);
            }
        }

        private static void ImportHtmlblock(string sitePath, WebSite website, RenderContext context)
        {
            var dir = Path.Combine(sitePath, "result", "htmlblock");
            if (!Directory.Exists(dir)) return;
            var htmlblocks = Directory.GetFiles(dir);

            foreach (var htmlblock in htmlblocks)
            {
                ImportLogger.Write($"导入htmlblock {htmlblock}");

                context.Request.Body = JsonHelper.Serialize(new
                {
                    values = new Dictionary<string, object> {
                        {website.DefaultCulture,File.ReadAllText(htmlblock)}
                    },
                    name = Path.GetFileNameWithoutExtension(htmlblock)
                }); ;

                var call = new ApiCall
                {
                    ObjectId = default(Guid),
                    WebSite = website,
                    Context = context
                };

                new HtmlBlockApi().Post(call);
            }
        }

        private static void ImportData(string sitePath, WebSite newsite, RenderContext context)
        {
            var data = File.ReadAllText(Path.Combine(sitePath, "result", "data.json"));
            var json = JsonHelper.DeserializeJObject(data);
            foreach (var item in json.Properties())
            {

            }
        }

        private static void ImportFiles(string sitePath, WebSite newsite, RenderContext context)
        {
            var dir = Path.Combine(sitePath, "static");
            if (!Directory.Exists(dir)) return;
            var files = Directory.GetFiles(dir);

            foreach (var file in files)
            {
                ImportLogger.Write($"导入file {file}");
                var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(File.ReadAllBytes(file)), "'file_0'", $"'{Path.GetFileName(file)}'");
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
            var dir = Path.Combine(sitePath, "image");
            if (!Directory.Exists(dir)) return;
            var images = Directory.GetFiles(dir);
            foreach (var image in images)
            {
                ImportLogger.Write($"导入image {image}");
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
            var dir = Path.Combine(sitePath, "css");
            if (!Directory.Exists(dir)) return;
            var styles = Directory.GetFiles(dir);

            foreach (var style in styles)
            {
                ImportLogger.Write($"导入css {style}");

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
            var dir = Path.Combine(sitePath, "js");
            if (!Directory.Exists(dir)) return;
            var scripts = Directory.GetFiles(dir);

            foreach (var script in scripts)
            {
                ImportLogger.Write($"导入js {script}");
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

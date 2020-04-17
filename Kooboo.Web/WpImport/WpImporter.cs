using Kooboo.Api;
using Kooboo.Data.Context;
using Kooboo.Data.Models;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Extensions;
using Kooboo.Web.Api;
using Kooboo.Web.Api.Implementation;
using Kooboo.Web.Areas.Admin.ViewModels;
using Kooboo.Web.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                var datas = ImportData(sitePath, website, call.Context);
                ImportViews(sitePath, website, call.Context, datas);
                ImportHtmlblock(sitePath, website, call.Context);
                ImportMenus(sitePath, website, call.Context);
                ImportLayouts(sitePath, website, call.Context);
                ImportPages(sitePath, website, call.Context);
            }
            catch (Exception ex)
            {
                ImportLogger.Write($"----------导入失败 {ex}");
                if (website != null) Kooboo.Sites.Service.WebSiteService.Delete(website.Id);
            }
        }

        private static void ImportMenus(string sitePath, WebSite website, RenderContext context)
        {
            var dir = Path.Combine(sitePath, "result", "menu");
            if (!Directory.Exists(dir)) return;
            var menus = Directory.GetFiles(dir, "*.json");

            foreach (var menu in menus)
            {
                ImportLogger.Write($"导入menu {menu}");
                var menuJson = JsonHelper.DeserializeJObject(File.ReadAllText(menu));
                new MenuImporter().Import(website, menuJson, context, Path.GetFileNameWithoutExtension(menu));
            }
        }

        private static void ImportPages(string sitePath, WebSite website, RenderContext context)
        {
            var dir = Path.Combine(sitePath, "result", "pages");
            if (!Directory.Exists(dir)) return;
            var pages = Directory.GetFiles(dir);

            foreach (var page in pages)
            {
                ImportLogger.Write($"导入page {page}");

                context.Request.Model = new PageUpdateViewModel
                {
                    UrlPath = $"/{Path.GetFileNameWithoutExtension(page)}",
                    Body = File.ReadAllText(page),
                    Name = Path.GetFileNameWithoutExtension(page),
                    Id = default(Guid)
                };

                var call = new ApiCall
                {
                    WebSite = website,
                    Context = context
                };

                call.Context.WebSite = website;

                new PageApi().Post(call);
            }
        }

        private static void ImportLayouts(string sitePath, WebSite website, RenderContext context)
        {
            var dir = Path.Combine(sitePath, "result", "layout");
            if (!Directory.Exists(dir)) return;
            var layouts = Directory.GetFiles(dir);

            foreach (var layout in layouts)
            {
                ImportLogger.Write($"导入layout {layout}");

                context.Request.Model = new Kooboo.Sites.Models.Layout
                {
                    Body = File.ReadAllText(layout),
                    Name = Path.GetFileNameWithoutExtension(layout),
                    Id = default(Guid)
                };

                var call = new ApiCall
                {
                    WebSite = website,
                    Context = context
                };

                new LayoutApi().Post(call);
            }
        }

        private static void ImportViews(string sitePath, WebSite website, RenderContext context, Dictionary<string, Guid> datas)
        {
            var dir = Path.Combine(sitePath, "result", "view");
            if (!Directory.Exists(dir)) return;
            var views = Directory.GetFiles(dir);


            foreach (var view in views)
            {
                ImportLogger.Write($"导入view {view}");
                var body = File.ReadAllText(view);

                var model = new ViewEditViewModel
                {
                    Body = body,
                    Name = Path.GetFileNameWithoutExtension(view),
                    Id = default(Guid),
                    DataSources = new List<Sites.Models.ViewDataMethod>()
                };

                var containsData = datas.Where(w => body.Contains(w.Key));

                if (containsData.Count() > 0)
                {
                    context.Request.Body = JsonHelper.Serialize(new
                    {
                        Id = Guid.Empty
                    });

                    foreach (var data in containsData)
                    {
                        var method = GetByFolderMethod(website, context);
                        method.Property("id").Value = Guid.Empty.ToString();
                        var parameterBinding = method.Property("parameterBinding").Value as JObject;
                        var folderId = parameterBinding.Property("folderId").Value as JObject;
                        folderId.Property("binding").Value = data.Value.ToString();
                        context.Request.Body = JsonHelper.Serialize(method);

                        var typeInfoModel = new DataMethodSettingApi().Update(new ApiCall
                        {
                            WebSite = website,
                            Context = context
                        });

                        model.DataSources.Add(new Sites.Models.ViewDataMethod
                        {
                            AliasName = data.Key,
                            MethodId = typeInfoModel.Id
                        });
                    }
                }


                context.Request.Model = model;

                var call = new ApiCall
                {
                    WebSite = website,
                    Context = context
                };

                new ViewApi().Post(call);
            }
        }

        private static JObject GetByFolderMethod(WebSite website, RenderContext context)
        {
            context.Request.Body = JsonHelper.Serialize(new
            {
                Id = Guid.Empty
            });

            var vms = new DataMethodSettingApi().ByView(new ApiCall
            {
                WebSite = website,
                Context = context
            });

            var byFolder = vms.SelectMany(s => s.Methods).First(f => f.MethodName == "ByFolder");

            context.Request.Body = JsonHelper.Serialize(new
            {
                byFolder.Id
            });

            var dataMethodSetting = new DataMethodSettingApi().Get(new ApiCall
            {
                WebSite = website,
                Context = context
            });

            return JsonHelper.DeserializeJObject(JsonHelper.Serialize(dataMethodSetting));
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

        private static Dictionary<string, Guid> ImportData(string sitePath, WebSite website, RenderContext context)
        {
            var file = Path.Combine(sitePath, "result", "data.json");
            var dic = new Dictionary<string, Guid>();
            if (!File.Exists(file)) return dic;
            var data = File.ReadAllText(file);
            var json = JsonHelper.DeserializeJObject(data);

            foreach (var item in json.Properties())
            {
                var result = new ContentImporter().Import(website, item, context);
                dic.Add(result.Key, result.Value);
            }

            return dic;
        }

        private static void ImportFiles(string sitePath, WebSite website, RenderContext context)
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
                    WebSite = website,
                    Context = context
                };

                new UploadApi().File(call);
            }
        }

        private static void ImportImages(string sitePath, WebSite website, RenderContext context)
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
                    WebSite = website,
                    Context = context
                };

                new UploadApi().Image(call);
            }
        }

        private static void ImportStyles(string sitePath, WebSite website, RenderContext context)
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
                    WebSite = website,
                    Context = context
                };

                new StyleApi().Update(call);
            }
        }

        private static void ImportScripts(string sitePath, Data.Models.WebSite website, RenderContext context)
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
                    WebSite = website,
                    Context = context
                };

                new ScriptApi().Update(call);
            }
        }
    }
}

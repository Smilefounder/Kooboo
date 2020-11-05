//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using System.Threading.Tasks;
using Kooboo.Extensions;
using Kooboo.Sites.Extensions;
using Kooboo.Lib;
using Kooboo.Lib.Helper;
using System;
using System.Linq;

namespace Kooboo.Sites.Render
{
    public class FileRenderer
    {
        public static void Render(FrontContext context)
        {
            var file = context.SiteDb.Files.Get(context.Route.objectId);
            if (file == null)
            {
                return;
            }

            RenderFile(context, file);
        }

        public static async Task RenderAsync(FrontContext context)
        {
            var file = await context.SiteDb.FilePool.GetAsync(context.Route.objectId);
            if (file == null)
            {
                return;
            }
            RenderFile(context, file);
        }

        public static void RenderFile(FrontContext context, Models.CmsFile file)
        {
            string contentType;

            if (!string.IsNullOrEmpty(file.ContentType))
            {
                contentType = file.ContentType;
            }
            else
            {
                contentType = IOHelper.MimeType(file.Name);
            }

            context.RenderContext.Response.ContentType = contentType;

            if (file.ContentBytes != null)
            {
                var range = context.RenderContext.Request.Headers.Get("range");
                if (range != null)
                {
                    var arr = range.Substring(6).Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s)).ToArray();
                    var start = arr[0];
                    if (arr.Length > 1)
                    {
                        var end = arr[1];
                        context.RenderContext.Response.Body = file.ContentBytes.Skip(start).Take(end - start).ToArray();
                        context.RenderContext.Response.Headers.Add("Accept-Ranges", "bytes");
                        context.RenderContext.Response.Headers.Add("Content-Range", $"bytes {start}-{end}/{file.ContentBytes.Length}");
                        if (end - start <= 0)
                        {
                            context.RenderContext.Response.StatusCode = 206;
                        }
                    }
                    else
                    {
                        context.RenderContext.Response.Body = file.ContentBytes;
                    }
                }
                else
                {
                    context.RenderContext.Response.Body = file.ContentBytes;
                }
            }
            else if (!string.IsNullOrEmpty(file.ContentString))
            {
                context.RenderContext.Response.Body = DataConstants.DefaultEncoding.GetBytes(file.ContentString);
            }

            // cache for font.
            if (contentType != null)
            {

                if (contentType.ToLower().Contains("font"))
                {
                    context.RenderContext.Response.Headers["Expires"] = DateTime.UtcNow.AddYears(1).ToString("r");
                }
                else if (contentType.ToLower().Contains("image"))
                {
                    context.RenderContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.RenderContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");

                    if (context.RenderContext.WebSite.EnableImageBrowserCache)
                    {
                        if (context.RenderContext.WebSite.ImageCacheDays > 0)
                        {
                            context.RenderContext.Response.Headers["Expires"] = DateTime.UtcNow.AddDays(context.RenderContext.WebSite.ImageCacheDays).ToString("r");
                        }
                        else
                        {
                            // double verify...
                            var version = context.RenderContext.Request.GetValue("version");
                            if (!string.IsNullOrWhiteSpace(version))
                            {
                                context.RenderContext.Response.Headers["Expires"] = DateTime.UtcNow.AddYears(1).ToString("r");
                            }
                        }
                    }

                }
            }


        }
    }
}

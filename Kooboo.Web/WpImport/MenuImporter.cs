using dotless.Core.Parser.Tree;
using Kooboo.Api;
using Kooboo.Data.Context;
using Kooboo.Data.Models;
using Kooboo.Lib.Helper;
using Kooboo.Web.Api.Implementation;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Kooboo.Web.WpImport
{
    public class MenuImporter
    {
        public void Import(WebSite webSite, JObject menuJson, RenderContext context, string menuName)
        {
            context.Request.Body = JsonHelper.Serialize(new
            {
                Name = menuName
            });

            var call = new ApiCall
            {
                WebSite = webSite,
                Context = context
            };

            var siteMenu = new MenuApi().Create(call);

            UpdateTemplate(menuJson, context, webSite, siteMenu.Id.ToString(), siteMenu.Id.ToString());
            AddSubMenu(menuJson, context, webSite, siteMenu.Id.ToString(), siteMenu.Id.ToString());
        }

        private static void AddSubMenu(JObject menuJson, RenderContext context, WebSite webSite, string parentId, string rootId)
        {
            var items = menuJson.Property("items");
            var itemArray = items?.Value as JArray;

            if (itemArray != null)
            {
                foreach (var item in itemArray)
                {
                    var jObject = item as JObject;
                    var data = jObject.Property("data")?.Value as JObject;

                    context.Request.Body = JsonHelper.Serialize(new
                    {
                        ParentId = parentId,
                        Name = data?.Property("anchortext")?.Value?.ToString(),
                        Url = data?.Property("href")?.Value?.ToString(),
                        RootId = rootId
                    });

                    var menu = new MenuApi().CreateSub(new ApiCall
                    {
                        WebSite = webSite,
                        Context = context
                    });

                    UpdateTemplate(jObject, context, webSite, menu.Id.ToString(), rootId);
                    AddSubMenu(jObject, context, webSite, menu.Id.ToString(), rootId);
                }
            }
        }

        private static void UpdateTemplate(JObject menuJson, RenderContext context, WebSite webSite, string id, string rootId)
        {
            var subItemTemplate = @"<li><a href=""{href}"">{anchortext}</a>{items}</li>";
            var items = menuJson.Property("items");
            var itemArray = items?.Value as JArray;
            var hasSubMenu = itemArray != null && itemArray.Count > 0;

            if (hasSubMenu)
            {
                subItemTemplate = (itemArray.First() as JObject).Property("template").Value.ToString();
            }

            context.Request.Body = JsonHelper.Serialize(new
            {
                Id = id,
                SubItemTemplate = subItemTemplate,
                SubItemContainer = id == rootId ? menuJson.Property("template").Value.ToString() : "{items}",
                Template = id == rootId ? null : menuJson.Property("template").Value.ToString(),
                RootId = rootId
            });


            new MenuApi().UpdateTemplate(new ApiCall
            {
                WebSite = webSite,
                Context = context
            });
        }
    }
}

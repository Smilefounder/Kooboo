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

            UpdateTemplate(menuJson, call, siteMenu);

            AddSubMenu(menuJson, call, siteMenu.Id.ToString(), siteMenu.Id.ToString());
        }

        private static void AddSubMenu(JObject menuJson, ApiCall call, string parentId, string rootId)
        {
            var items = menuJson.Property("items");
            var itemArray = items?.Value as JArray;

            if (itemArray != null)
            {
                foreach (var item in itemArray)
                {
                    var jObject = item as JObject;
                    var data = jObject.Property("data")?.Value as JObject;

                    call.Context.Request.Body = JsonHelper.Serialize(new
                    {
                        ParentId = parentId,
                        Name = data.Property("href").Value.ToString(),
                        Url = data.Property("anchortext").Value.ToString(),
                        RootId = rootId
                    });

                    var menu = new MenuApi().CreateSub(call);
                    UpdateTemplate(jObject, call, menu);
                    AddSubMenu(jObject, call, menu.Id.ToString(), rootId);
                }
            }
        }

        private static void UpdateTemplate(JObject menuJson, ApiCall call, Sites.Models.Menu siteMenu)
        {
            var subItemTemplate = "<li><a href='{ href}'>{anchortext}</a>{items}</li>";
            var items = menuJson.Property("items");
            var itemArray = items?.Value as JArray;

            if (itemArray != null && itemArray.Count > 0)
            {
                subItemTemplate = (items.First() as JObject).Property("template").Value.ToString();
            }

            call.Context.Request.Body = JsonHelper.Serialize(new
            {
                Template = "",
                SubItemTemplate = subItemTemplate,
                SubItemContainer = menuJson.Property("template").Value.ToString(),
                RootId = siteMenu.Id.ToString()
            });

            new MenuApi().UpdateTemplate(call);
        }
    }
}

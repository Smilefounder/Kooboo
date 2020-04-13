using Kooboo.Api;
using Kooboo.Data.Context;
using Kooboo.Data.Models;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Contents.Models;
using Kooboo.Sites.Extensions;
using Kooboo.Sites.Repository;
using Kooboo.Web.Api.Implementation;
using Kooboo.Web.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.WpImport
{
    public class ContentImporter
    {
        public void Import(WebSite webSite, JProperty jProperty, RenderContext context)
        {
            CreateContentType(webSite, jProperty, context);
        }

        private Guid CreateContentType(WebSite webSite, JProperty jProperty, RenderContext context, string prefix = "")
        {
            var jArray = jProperty.Value as JArray;

            var properies = jArray.SelectMany(s => (s as JObject).Properties())
                                  .GroupBy(g => g.Name).Select(s => s.OrderByDescending(o => o.Value.ToString().Length)
                                  .FirstOrDefault());

            var buildInTypes = properies.Where(a => a.Value is JArray);
            var name = $"{(prefix == string.Empty ? string.Empty : $"{prefix}_")}{jProperty.Name}";

            var props = properies.Except(buildInTypes).Select(s => new ContentProperty
            {
                Name = s.Name,
                DisplayName = s.Name,
                ControlType = s.Value.ToString().Length > 100 ? "TextArea" : "TextBox",
                DataType = Data.Definition.DataTypes.String,
                Editable = true,
                MaxLength = (s.Value.ToString().Length / 1024 + 1) * 1024
            }).ToList();

            context.Request.Model = new ContentType
            {
                Id = default(Guid),
                Name = $"{name}{(buildInTypes.Count() > 0 ? "_base" : string.Empty)}",
                Properties = props
            };

            var call = new ApiCall
            {
                ObjectId = default(Guid),
                WebSite = webSite,
                Context = context
            };

            var baseId = new ContentTypeApi().Post(call);
            var folderId = GetFolderId(webSite, baseId);
            var embeddedFolders = new List<EmbeddedFolder>();

            if (buildInTypes.Count() > 0)
            {
                foreach (var item in buildInTypes)
                {
                    var id = CreateContentType(webSite, item, context, name);

                    embeddedFolders.Add(new EmbeddedFolder
                    {
                        FolderId = id,
                        Alias = item.Name
                    });
                }

                context.Request.Model = new CreateContentFolderViewModel
                {
                    Id = default(Guid),
                    Name = name,
                    ContentTypeId = baseId,
                    Embedded = embeddedFolders
                };

                folderId = new ContentFolderApi().Post(call);
            };

            foreach (var item in jArray)
            {
                var value = item as JObject;
                var valueProps = value.Properties();

                var textContent = new LangTextContentViewModel
                {
                    Id = Guid.Empty.ToString(),
                    FolderId = folderId.ToString(),
                    Values = new Dictionary<string, Dictionary<string, string>> {
                        {webSite.DefaultCulture, valueProps.Where(w=>!(w.Value is JArray)).ToDictionary(t => t.Name, t => t.Value.ToString())}
                    },
                    Embedded = new Dictionary<Guid, List<Guid>>
                    {
                        {Guid.Empty,new List<Guid>() }
                    }
                };

                context.Request.Model = textContent;
                new TextContentApi().LangUpdate(call);
            }

            return folderId;
        }

        Guid GetFolderId(WebSite webSite, Guid contentTypeId)
        {
            return webSite.SiteDb().ContentFolders.Query.Where(w => w.ContentTypeId == contentTypeId).FirstOrDefault().Id;
        }
    }
}

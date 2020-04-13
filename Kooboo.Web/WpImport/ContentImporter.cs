using Kooboo.Api;
using Kooboo.Data.Context;
using Kooboo.Data.Models;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Contents.Models;
using Kooboo.Sites.Extensions;
using Kooboo.Web.Api.Implementation;
using Kooboo.Web.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Web.WpImport
{
    public class ContentImporter
    {
        public void Import(WebSite webSite, JProperty jProperty, RenderContext context)
        {
            var id = CreateContentType(webSite, jProperty, context);
            AddData(id, webSite, jProperty, context);
        }

        private List<Guid> AddData(Guid id, WebSite webSite, JProperty jProperty, RenderContext context)
        {
            context.Request.Body = JsonHelper.Serialize(new
            {
                FolderId = id,
                Id = default(Guid)
            });

            var call = new ApiCall
            {
                ObjectId = default(Guid),
                WebSite = webSite,
                Context = context
            };

            var vm = new TextContentApi().GetEdit(call);
            var valueIds = new List<Guid>();

            foreach (var item in jProperty.Value as JArray)
            {
                var value = item as JObject;
                var properies = value.Properties();
                var buildInTypes = properies.Where(a => a.Value is JArray);

                var textContent = new LangTextContentViewModel
                {
                    Id = Guid.Empty.ToString(),
                    FolderId = id.ToString(),
                    Values = new Dictionary<string, Dictionary<string, string>> {
                        {webSite.DefaultCulture, properies.Except(buildInTypes).ToDictionary(t => t.Name, t => t.Value.ToString())}
                    }
                };

                var embeddedValues = new Dictionary<Guid, List<Guid>>();

                foreach (var buildInType in buildInTypes)
                {
                    var folderId = vm.Embedded.Find(f => f.Alias == buildInType.Name).EmbeddedFolder.Id;
                    embeddedValues.Add(folderId, AddData(folderId, webSite, buildInType, context));
                }

                textContent.Embedded = embeddedValues;
                context.Request.Model = textContent;

                context.Request.Body = JsonHelper.Serialize(new
                {
                    FolderId = id,
                    Id = default(Guid)
                });

                valueIds.Add(new TextContentApi().LangUpdate(call));
            }

            return valueIds;
        }

        private Guid CreateContentType(WebSite webSite, JProperty jProperty, RenderContext context, string prefix = "")
        {
            var jArray = jProperty.Value as JArray;

            var properies = jArray.SelectMany(s => (s as JObject).Properties())
                                  .GroupBy(g => g.Name).Select(s => s.OrderByDescending(o => o.Value.ToString().Length)
                                  .FirstOrDefault());

            var buildInTypes = properies.Where(a => a.Value is JArray);
            var name = $"{(prefix == string.Empty ? string.Empty : $"{prefix}_")}{jProperty.Name}";
            var props = GetContentProperties(properies.Except(buildInTypes));

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

            return folderId;
        }

        private static List<ContentProperty> GetContentProperties(IEnumerable<JProperty> properies)
        {
            return properies.Select(s => new ContentProperty
            {
                Name = s.Name,
                DisplayName = s.Name,
                ControlType = s.Value.ToString().Length > 100 ? "TextArea" : "TextBox",
                DataType = Data.Definition.DataTypes.String,
                Editable = true,
                MaxLength = (s.Value.ToString().Length / 1024 + 1) * 1024
            }).ToList();
        }

        Guid GetFolderId(WebSite webSite, Guid contentTypeId)
        {
            return webSite.SiteDb().ContentFolders.Query.Where(w => w.ContentTypeId == contentTypeId).FirstOrDefault().Id;
        }
    }
}

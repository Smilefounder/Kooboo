
using Kooboo.Lib.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using static Kooboo.Sites.Commerce.Entities.Category;

namespace Kooboo.Sites.Commerce.Models.Category
{
    public class CategoryModel
    {
        public CategoryModel()
        {

        }

        public CategoryModel(Entities.Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Type = category.Type;
            if (!string.IsNullOrWhiteSpace(category.Rule)) Rule = JsonHelper.Deserialize<MatchRule.Rule>(category.Rule);
            Enable = category.Enable;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AddingType Type { get; set; }
        public MatchRule.Rule Rule { get; set; }
        public bool Enable { get; set; }

        public Entities.Category ToCategory()
        {
            return new Entities.Category
            {
                Id = Id,
                Name = Name,
                Rule = JsonHelper.Serialize(Rule),
                Type = Type,
                Enable = Enable,
                CreateTime = DateTime.UtcNow
            };
        }
    }

}

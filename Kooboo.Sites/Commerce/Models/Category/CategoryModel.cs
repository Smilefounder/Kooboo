
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using static Kooboo.Sites.Commerce.Entities.Category;

namespace Kooboo.Sites.Commerce.Models.Category
{
    public class CategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AddingType Type { get; set; }
        public MatchRule.Rule Rule { get; set; }
    }

}

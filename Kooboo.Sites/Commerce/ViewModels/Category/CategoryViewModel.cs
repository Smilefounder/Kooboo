using Kooboo.Data.Definition;
using Kooboo.Lib.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using static Kooboo.Sites.Commerce.Entities.Category;

namespace Kooboo.Sites.Commerce.ViewModels.Category
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AddingType Type { get; set; }
        public MatchRule.Rule Rule { get; set; }
    }

}

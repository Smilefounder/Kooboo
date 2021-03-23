using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule
{
    public class Condition
    {
        public Guid Id { get; set; }
        public string Left { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Comparer Comparer { get; set; }
        public string Right { get; set; }
    }
}

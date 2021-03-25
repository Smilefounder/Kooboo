using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.MatchRule
{
    public class Rule
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public MatchingType Type { get; set; }
        public Condition[] Conditions { get; set; }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Entities
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public AddingType Type { get; set; }
        public string Rule { get; set; }

        [NotUpdate]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public bool Enable { get; set; }

        public enum AddingType
        {
            Manual = 0,
            Auto = 1
        }
    }
}

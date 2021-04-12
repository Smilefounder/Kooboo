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
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        public enum AddingType
        {
            Manual = 0,
            Auto = 1
        }
    }
}

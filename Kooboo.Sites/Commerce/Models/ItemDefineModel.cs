using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Models
{
    public class ItemDefineModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DefineType Type { get; set; }
        public KeyValuePair<Guid, string>[] Options { get; set; }
        public enum DefineType
        {
            Text = 0,
            Option = 1
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Id);
            sb.Append(Type.ToString());

            if (Type == DefineType.Option)
            {
                var options = Options.OrderBy(o => o.Key);

                foreach (var item in Options)
                {
                    sb.Append(item.Key);
                }
            }

            return sb.ToString();
        }
    }

}

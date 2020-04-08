using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kooboo.Sites.Logistics.Models
{
    public class LogisticsStatusResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderStatus Status
        {
            get; set;
        }

        public string Message { get; set; }
    }
}

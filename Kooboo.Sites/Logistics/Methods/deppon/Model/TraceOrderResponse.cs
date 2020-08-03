using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.deppon.Model
{
    public class TraceOrderResponse
    {
        public string Result { get; set; }

        public Traces ResponseParam { get; set; }
    }

    public class Traces
    {
        [JsonProperty("trace_list")]
        public List<Trace> TraceList { get; set; }
    }

    public class Trace
    {
        public string Description { get; set; }

        public string Status { get; set; }

        public string Site { get; set; }

        public string Time { get; set; }
    }
}

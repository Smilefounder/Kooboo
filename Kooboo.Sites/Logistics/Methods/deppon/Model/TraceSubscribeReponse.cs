using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Kooboo.Sites.Logistics.Methods.deppon.Model
{
    public class TraceSubscribeReponse
    {
        [JsonProperty("track_list")]
        public List<TraceMsgs> TrackList { get; set; }
    }

    public class TraceMsgs
    {
        [JsonProperty("trace_list")]
        public List<Trace> TraceList { get; set; }

        [JsonProperty("tracking_number")]
        public string Number { get; set; }
    }
}

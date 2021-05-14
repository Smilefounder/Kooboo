using Kooboo.Sites.FrontEvent;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Models
{
    [Kooboo.Attributes.Diskable]
    public class OpenApi : CoreObject
    {
        public string JsonData { get; set; }
        public string Url { get; set; }
        public bool IsRemote { get; set; }

        //TODO Cache

        public override int GetHashCode()
        {
            var un = Name;
            un += Online.ToString();
            un += JsonData;
            un += Url;
            un += IsRemote;

            return Lib.Security.Hash.ComputeIntCaseSensitive(un);
        }
    }
}

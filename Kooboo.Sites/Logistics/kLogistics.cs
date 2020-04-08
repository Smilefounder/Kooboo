using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Data.Interface;

namespace Kooboo.Sites.Logistics
{
    public class kLogistics : IkScript
    {

        [KIgnore]
        public string Name => "Logistics";

        [KIgnore]
        public RenderContext context { get; set; }

        [KIgnore]
        public KLogisticsMethodWrapper this[string key]
        {
            get
            {
                return Get(key);
            }
        }

        public KLogisticsMethodWrapper Get(string logisticsName)
        {
            var logisticsmethod = LogisticsManager.GetMethod(logisticsName, this.context);
            if (logisticsmethod != null)
            {
                KLogisticsMethodWrapper method = new KLogisticsMethodWrapper(logisticsmethod, this.context);
                return method;
            }
            return null;
        }

        [KExtension]
        static KeyValuePair<string, Type>[] _ = LogisticsContainer.LogisticsMethods.Select(s => new KeyValuePair<string, Type>(s.Name, s.GetType())).ToArray();

    }
}

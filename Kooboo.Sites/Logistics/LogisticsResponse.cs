using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data.Attributes;

namespace Kooboo.Sites.Logistics
{
    public class LogisticsResponse : ILogisticsResponse
    {
        public Guid requestId { get; set; }

        public string logisticsMethodReferenceId { get; set; }


        private KScript.KDictionary _fieldvalues;
        public KScript.KDictionary fieldValues
        {
            get
            {
                if (_fieldvalues == null)
                {
                    _fieldvalues = new KScript.KDictionary();
                }
                return _fieldvalues;
            }
            set
            {
                _fieldvalues = value;
            }
        }

        [KIgnore]
        public void setFieldValues(Dictionary<string, string> input)
        {
            foreach (var item in input)
            {
                setFieldValues(item.Key, item.Value);
            }
        }
        [KIgnore]
        public void setFieldValues(string key, string value)
        {
            this.fieldValues.Add(key, value);
        }
    }
}

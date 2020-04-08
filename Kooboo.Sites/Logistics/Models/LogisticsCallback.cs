using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Kooboo.Data;
using Kooboo.Data.Attributes;
using Kooboo.Data.Interface;
using Kooboo.IndexedDB.CustomAttributes;
using Kooboo.Sites.Payment.Callback;

namespace Kooboo.Sites.Logistics.Models
{
    public class LogisticsCallback : IGolbalObject, ISiteObject
    {
        private Guid _id;

        public Guid Id
        {
            get
            {
                if (_id == default(Guid))
                    _id = Lib.Helper.IDHelper.NewTimeGuid(DateTime.UtcNow);
                return _id;
            }
            set { _id = value; }
        }
        public Guid RequestId { get; set; }

        public string ResponseMessage { get; set; }

        public string RawData { get; set; }

        [Description("The call back relative url includes query string")]
        public string RequestUrl { get; set; }

        [Description("Form post data")]
        public string PostData { get; set; }

        [Description("Orginal request data")]
        public KScript.KDictionary Request;

        [KoobooIgnore]
        [KIgnore]
        public CallbackResponse CallbackResponse
        {
            get; set;
        }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [KIgnore]
        public byte ConstType { get; set; } = ConstObjectType.PaymentCallback;

        public DateTime LastModified { get; set; }

        public string Name { get; set; }
    }
}

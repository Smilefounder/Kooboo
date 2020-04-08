using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data;
using Kooboo.Data.Attributes;
using Kooboo.Sites.Models;

namespace Kooboo.Sites.Logistics.Models
{
    public class LogisticsRequest : CoreObject, IGolbalObject
    {
        public LogisticsRequest()
        {
            this.ConstType = ConstObjectType.LogisticsRequest;
        }

        private Guid _id;

        public override Guid Id
        {
            get
            {
                if (_id == default(Guid))
                {
                    _id = Lib.Helper.IDHelper.NewTimeGuid(DateTime.Now);
                }
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        //additional info if needed. 
        public string Description { get; set; }

        [KIgnore]
        [Obsolete]
        public Guid OrganizationId { get; set; }

        [KIgnore]
        [Obsolete]
        public Guid WebSiteId { get; set; }

        public Infos SenderInfo { get; set; }

        public Infos ReceiverInfo { get; set; }

        public double Weight { get; set; }

        public double Length { get; set; }

        public double Height { get; set; }

        public Guid OrderId { get; set; }

        public string UserIp { get; set; }

        public string LogisticsMethod { get; set; }

        public bool Created { get; set; }

        public bool Failed { get; set; }

        public OrderStatus Status { get; set; }

        /// <summary>
        /// The reference id at the payment provider if any. 
        /// </summary>
        public string ReferenceId { get; set; }

        public string ReturnPath { get; set; }

        private Dictionary<string, object> _Additional;
        public Dictionary<string, object> Additional
        {
            get
            {
                if (_Additional == null)
                {
                    _Additional = new Dictionary<string, object>();
                }
                return _Additional;
            }
            set
            {
                _Additional = value;
            }
        }

    }

    public class Infos
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Phone { get; set; }

        public string Prov { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string Address { get; set; }
    }
}


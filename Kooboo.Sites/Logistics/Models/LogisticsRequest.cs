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

        [KIgnore]
        [Obsolete]
        public Guid OrganizationId { get; set; }

        [KIgnore]
        [Obsolete]
        public Guid WebSiteId { get; set; }

        public Info SenderInfo { get; set; }

        public Info ReceiverInfo { get; set; }

        public CargoInfo CargoInfo { get; set; }

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
        /// The reference id at the logistics provider if any. 
        /// </summary>
        public string ReferenceId { get; set; }

        private Guid _referenceIdHash;
        public Guid ReferenceIdHash
        {
            get
            {
                if (_referenceIdHash == default(Guid))
                {
                    if (!string.IsNullOrWhiteSpace(this.ReferenceId))
                    {
                        _referenceIdHash = Lib.Security.Hash.ComputeHashGuid(this.ReferenceId);
                    }
                }
                return _referenceIdHash;
            }
            set
            {
                _referenceIdHash = value;
            }
        }

        public string Postage { get; set; }

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

    public class CargoInfo
    {
        public string Name { get; set; }

        public string Price { get; set; }

        public string Weight { get; set; }

        public string Count { get; set; }

        public string Currency { get; set; }

        public string Unit { get; set; }

        public string SourceArea { get; set; }
    }

    public class Info
    {
        public string Country { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Phone { get; set; }

        public string Prov { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string Address { get; set; }
    }
}


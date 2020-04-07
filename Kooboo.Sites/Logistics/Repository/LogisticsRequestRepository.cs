using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.IndexedDB;
using Kooboo.Sites.Logistics.Models;

namespace Kooboo.Sites.Logistics.Repository
{
    public class LogisticsRequestRepository : Kooboo.Sites.Repository.SiteRepositoryBase<LogisticsRequest>
    {
        public override ObjectStoreParameters StoreParameters
        {
            get
            {
                ObjectStoreParameters para = new ObjectStoreParameters();
                para.AddIndex<LogisticsRequest>(o => o.OrderId);
                para.AddColumn<LogisticsRequest>(o => o.Created);
                para.AddColumn<LogisticsRequest>(o => o.Failed);
                return para;
            }
        }

        public bool UpdateCreated(Guid ReqeustId)
        {
            var reqeust = this.Get(ReqeustId);
            if (reqeust != null)
            {
                this.Store.UpdateColumn<bool>(ReqeustId, o => o.Created, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateCancel(Guid ReqeustId)
        {
            var reqeust = this.Get(ReqeustId);
            if (reqeust != null)
            {
                this.Store.UpdateColumn<bool>(ReqeustId, o => o.Failed, true);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

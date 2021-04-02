using Dapper;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class ConsigneeService : ServiceBase
    {
        public ConsigneeService(RenderContext context) : base(context)
        {
        }

        public Consignee[] List(Guid customerId)
        {
            using (var con = DbConnection)
            {
                return con.Query<Consignee>("Select * from Consignee where CustomerId=@CustomerId", new { CustomerId = customerId }).ToArray();
            }
        }

        public Consignee Get(Guid id, IDbConnection connection = null)
        {
            var con = connection ?? DbConnection;
            return con.Get<Consignee>(id);
            if (connection == null) con.Dispose();
        }

        public void Save(Consignee consignee)
        {
            using (var con = DbConnection)
            {
                if (con.Exist<Consignee>(consignee.Id))
                {
                    con.Update(consignee);
                }
                else
                {
                    con.Insert(consignee);
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var con = DbConnection)
            {
                con.Delete<Consignee>(id);
            }
        }
    }
}

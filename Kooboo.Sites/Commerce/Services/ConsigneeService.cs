using Dapper;
using Kooboo.Sites.Commerce.Entities;
using System;
using System.Data;
using System.Linq;

namespace Kooboo.Sites.Commerce.Services
{
    public class ConsigneeService : ServiceBase
    {
        public ConsigneeService(SiteCommerce commerce) : base(commerce)
        {
           
        }

        public Consignee[] List(Guid customerId)
        {
            return Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                return con.Query<Consignee>("Select * from Consignee where CustomerId=@CustomerId", new { CustomerId = customerId }).ToArray();
            });
        }

        public Consignee Get(Guid id, IDbConnection connection = null)
        {
            return (connection ?? Commerce.CreateDbConnection()).ExecuteTask(con =>
              {
                  return con.Get<Consignee>(id);
              }, connection == null, connection == null);
        }

        public void Save(Consignee consignee)
        {
            Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                if (con.Exist<Consignee>(consignee.Id))
                {
                    con.Update(consignee);
                }
                else
                {
                    con.Insert(consignee);
                }
            });
        }

        public void Delete(Guid id)
        {
            Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                con.Delete<Consignee>(id);
            });
        }
    }
}

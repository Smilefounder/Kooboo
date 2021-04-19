using Dapper;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Kooboo.Sites.Commerce.Entities.Customer;

namespace Kooboo.Sites.Commerce.Services
{
    public class CustomerService : ServiceBase
    {
        public CustomerService(RenderContext context) : base(context)
        {
        }

        public void Register(string userName, string password)
        {
            if (userName?.Length < 3) throw new Exception("Name too short");
            if (password?.Length < 8) throw new Exception("Password too short");

            using (var con = DbConnection)
            {
                var exist = con.Query("SELECT 1 FROM Customer WHERE UserName = @Name LIMIT 1", new { Name = userName }).Any();
                if (exist) throw new Exception("Name exist");
                con.Insert(new Customer
                {
                    Id = Guid.NewGuid(),
                    UserName = userName?.Trim(),
                    Password = Kooboo.Lib.Security.Hash.ComputeHashGuid(password).ToString("N"),
                    CreateTime = DateTime.UtcNow
                });
            }
        }

        public PagedListModel<CustomerModel> List(PagingQueryModel model)
        {
            var result = new PagedListModel<CustomerModel>();

            using (var con = DbConnection)
            {
                var count = con.Count<Customer>();
                result.SetPageInfo(model, count);

                result.List = con.Query<CustomerModel>(@"
SELECT T.Id, T.UserName, T.Cart, T.CreateTime
FROM (SELECT c.id AS Id, UserName, SUM(CASE WHEN CI.SkuId IS NULL THEN 0 ELSE 1 END) AS Cart, c.CreateTime AS CreateTime
      FROM Customer C
               LEFT JOIN CartItem CI ON C.Id = CI.CustomerId
      GROUP BY c.Id) AS T
ORDER BY T.CreateTime DESC
LIMIT @Size OFFSET @Offset
", new
                {
                    model.Size,
                    Offset = result.GetOffset()
                }).ToList();
            }

            return result;
        }


    }
}

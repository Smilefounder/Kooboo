using Dapper;
using FluentValidation;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Customer;
using Kooboo.Sites.Commerce.Validators;
using System;
using System.Linq;

namespace Kooboo.Sites.Commerce.Services
{
    public class CustomerService : ServiceBase
    {
        public CustomerService(RenderContext context) : base(context)
        {
        }

        public void Register(CreateCustomerModel model)
        {
            new CreateCustomerModelValidator().ValidateAndThrow(model);

            DbConnection.ExecuteTask(con =>
            {
                var exist = con.QuerySingle<bool>(@"
SELECT EXISTS(
               SELECT 1
               FROM Customer
               WHERE UserName = @UserName
                  OR Phone = @Phone
                  OR Email = @Email
           )
", model);
                if (exist) throw new Exception("user exist");

                con.Insert(new Customer
                {
                    Id = Guid.NewGuid(),
                    UserName = model.UserName,
                    Password = Kooboo.Lib.Security.Hash.ComputeHashGuid(model.Password).ToString("N"),
                    Email = model.Email,
                    Phone = model.Phone
                });
            });
        }

        public PagedListModel<CustomerListModel> List(PagingQueryModel model)
        {
            var result = new PagedListModel<CustomerListModel>();

            DbConnection.ExecuteTask(con =>
            {
                var count = con.Count<Customer>();
                result.SetPageInfo(model, count);
                result.List = con.Query<CustomerListModel>(@"
SELECT T.*, SUM(CASE WHEN CI.SkuId IS NULL THEN 0 ELSE 1 END) AS Cart
FROM (SELECT C.Id, C.Email, C.Phone, C.UserName, C.CreateTime
      FROM Customer C
      LIMIT @Size OFFSET @Offset) T
         LEFT JOIN CartItem CI ON CI.CustomerId = T.Id
GROUP BY T.Id
", new
                {
                    model.Size,
                    Offset = result.GetOffset()
                }).ToList();
            });

            return result;
        }
    }
}

using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class OrderService : ServiceBase
    {
        public OrderService(RenderContext context) : base(context)
        {
        }

        public void CreateOrder(CreateOrderModel model)
        {
            var cart = new CartService(Context).GetCart(model.CustomerId);

            if (cart.Items.Any(a => a.Quantity > a.Stock))
            {
                throw new Exception("We are out of stock, please renew your order");
            }

            using (var con = DbConnection)
            {
                con.Open();
                var tran = con.BeginTransaction();



                tran.Commit();
            }
        }
    }
}

using Kooboo.Api;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models.Cart;
using System;
using System.Collections.Generic;
using System.Text;
using static Kooboo.Sites.Commerce.Entities.Customer;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class CartApi : IApi
    {
        public string ModelName => "Cart";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public void Post(CartItem cartItem, ApiCall apiCall)
        {
            new CartService(apiCall.Context).SaveItem(cartItem);
        }

        public CartModel Get(Guid id, ApiCall apiCall)
        {
            return new CartService(apiCall.Context).GetCart(id);
        }

        public void Deletes(Guid[] ids, ApiCall apiCall)
        {
            new CartService(apiCall.Context).DeleteItems(ids);
        }
    }
}

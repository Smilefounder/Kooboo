using Kooboo.Api;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.ViewModels.Cart;
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

        public CartViewModel Get(Guid id, ApiCall apiCall)
        {
            return new CartService(apiCall.Context).GetCart(id);
        }
    }
}

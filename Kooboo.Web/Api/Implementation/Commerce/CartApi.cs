using Kooboo.Api;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models.Cart;
using System;
using Kooboo.Sites.Commerce.Models;

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
            var filterNotSelected = apiCall.GetBoolValue("filterNotSelected");
            return new CartService(apiCall.Context).GetCart(id, filterNotSelected: filterNotSelected);
        }

        public void Deletes(Guid[] ids, ApiCall apiCall)
        {
            new CartService(apiCall.Context).DeleteItems(ids);
        }

        public PagedListModel<CartListModel> List(PagingQueryModel model, ApiCall apiCall)
        {
            return new CartService(apiCall.Context).Query(model);
        }
    }
}

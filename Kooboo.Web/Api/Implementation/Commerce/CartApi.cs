using Kooboo.Api;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models.Cart;
using System;
using Kooboo.Sites.Commerce.Models;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class CartApi : CommerceApi
    {
        public override string ModelName => "Cart";

        public void Post(Guid customerId, Guid skuId, int quantity, bool selected, ApiCall apiCall)
        {
            GetService<CartService>(apiCall).Post(customerId, skuId, quantity, selected);
        }

        public CartModel Get(Guid id, ApiCall apiCall)
        {
            var filterNotSelected = apiCall.GetBoolValue("filterNotSelected");
            return GetService<CartService>(apiCall).GetCart(id, filterNotSelected: filterNotSelected);
        }

        public void Deletes(Guid[] ids, ApiCall apiCall)
        {
            GetService<CartService>(apiCall).DeleteItems(ids);
        }

        public PagedListModel<CartListModel> List(PagingQueryModel model, ApiCall apiCall)
        {
            return GetService<CartService>(apiCall).Query(model);
        }
    }
}

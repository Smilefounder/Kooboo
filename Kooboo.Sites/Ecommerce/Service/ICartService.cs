using Kooboo.Sites.Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.Service
{
    public interface ICartService: IEcommerceService<Cart>
    { 
        Cart GetOrCreateCart();

        void AddITem(Guid ProductVariantId, int quantity = 1);

        void RemoveItem(Guid variantId);

        void RemoveAll();

        void ChangeQuantity(Guid ProductVariantId, int newQuantity);

        /// <summary>
        /// update shopping cart and recalculate all promotions. 
        /// </summary>
        /// <param name="cart"></param>
        void UpdateCart(Cart cart);

        void CalculatePromotion(Cart cart);

        List<Cart> List(int skip, int count);

        List<Cart> List(int skip, DateTime? start = null, DateTime? end = null, string productMsg = null, string categaryId = null, int take = 10);

        int Count(); 

    }
}

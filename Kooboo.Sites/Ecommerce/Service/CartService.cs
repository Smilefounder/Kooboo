using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Data.Context;
using Kooboo.Sites.Ecommerce.Models;

namespace Kooboo.Sites.Ecommerce.Service
{
    public class CartService : ServiceBase<Cart>, ICartService
    {
        public Cart GetOrCreateCart()
        {
            var cart = this.Repo.Query.Where(o => o.CustomerId == this.CommerceContext.customer.Id).OrderByDescending().FirstOrDefault();
            if (cart != null && !cart.IsOrder)
            {
                return cart;
            }
            else
            {
                Cart newcart = new Cart();
                newcart.CustomerId = this.CommerceContext.customer.Id;
                this.Repo.AddOrUpdate(newcart);
                return newcart;
            }
        }

        public void AddITem(Guid ProductVariantId, int quantity = 1)
        {
            var variantService = ServiceProvider.GetService<ProductVariantsService>(this.Context);

            var variant = variantService.Get(ProductVariantId);

            if (variant != null)
            {
                var cart = GetOrCreateCart();
                var find = cart.Items.Find(o => o.ProductVariantId == ProductVariantId);
                if (find == null)
                {
                    CartItem item = new CartItem();
                    item.ProductVariantId = ProductVariantId;
                    item.UnitPrice = variant.Price;
                    item.ProductId = variant.ProductId;

                    cart.Items.Add(item);
                }
                else
                {
                    find.Quantity += 1;
                }

                UpdateCart(cart);
            }
        }

        public void RemoveItem(Guid variantId)
        {
            var cart = GetOrCreateCart();
            cart.Items.RemoveAll(o => o.ProductVariantId == variantId);
            UpdateCart(cart);
        }

        public void RemoveAll()
        {
            var cart = GetOrCreateCart();
            cart.Items.RemoveAll(it => true);
            UpdateCart(cart);
        }

        public void ChangeQuantity(Guid ProductVariantId, int newQuantity)
        {
            var variantService = ServiceProvider.GetService<ProductVariantsService>(this.Context);

            var variant = variantService.Get(ProductVariantId);

            if (variant != null)
            {
                var cart = GetOrCreateCart();
                var find = cart.Items.Find(o => o.ProductVariantId == ProductVariantId);
                if (find == null)
                {
                    CartItem item = new CartItem();
                    item.ProductVariantId = ProductVariantId;
                    item.UnitPrice = variant.Price;
                    item.ProductId = variant.ProductId;
                    item.Quantity = newQuantity;

                    cart.Items.Add(item);
                }
                else
                {
                    find.Quantity = newQuantity;
                }

                UpdateCart(cart);
            }
        }

        public void UpdateCart(Cart cart)
        {
            //calculate discount before updates...
            CalculatePromotion(cart);
            this.Repo.AddOrUpdate(cart);
        }

        public void CalculatePromotion(Cart cart)
        {
            Promotion.PromotionEngine.CalculatePromotion(cart, this.Context);
        }

        /// <summary>
        /// lastest list.
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<Cart> List(int skip, int take)
        {
            var list = this.Repo.Store.Where().OrderByDescending().Skip(skip).Take(take);
            return list;
        }

        public List<Cart> List(int skip, DateTime? start = null, DateTime? end = null, string productMsg = null, string categaryId = null, int take = 10)
        {
            var list = this.Repo.Store.Where();
            if (start != null)
            {
                list.Where(it => it.CreationDate >= start);
            }

            if (end != null)
            {
                list.Where(it => it.CreationDate <= end);
            }

            //TO DO according product name to get cart message
            //if (!string.IsNullOrEmpty(productMsg))
            //{
            //    var productService = ServiceProvider.GetService<ProductService>(this.Context);
            //    var product = productService.Repo.All().Where(it=>it.Body.Contains(productMsg)).FirstOrDefault();
            //    if (product != null)
            //    {
            //        list.Where(it => it.Items.Select(a => a.Id).Contains(product.Id));
            //    }
            //}

            //TO DO according categary to get cart message
            //if (!string.IsNullOrEmpty(categaryId))
            //{
            //    var productService = ServiceProvider.GetService<ProductService>(this.Context);
            //    var productIds = productService.ByCategory(categaryId).Select(it=>it.Id).ToList();
            //    foreach (var productId in productIds)
            //    {
            //        list.Where(it => it.Items.Select(a => a.ProductId).Contains(productId));
            //    }
            //}

            return list.OrderByDescending().Skip(skip).Take(take);
        }

        public int Count()
        {
            return this.Repo.Store.Filter.Count();
        }
    }
}

using Kooboo.Sites.Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.Service
{
   public class ProductVariantsService : ServiceBase<ProductVariants>
    { 
        public List<ProductVariants> ListByProduct(Guid ProductId)
        {
           return this.Repo.Query.Where(o => o.ProductId == ProductId).SelectAll(); 
        }

        public bool DeductStock(Guid id, int quantity)
        {
            var variant = this.Get(id);
            if(variant.Stock < quantity)
            {
                return false;
            }
            variant.Stock = variant.Stock - quantity;
            return this.AddOrUpdate(variant);
        }

        public bool ReturnStock(Guid id, int quantity)
        {
            var variant = this.Get(id);
            variant.Stock = variant.Stock + quantity;
            return this.AddOrUpdate(variant);
        }
    }
}

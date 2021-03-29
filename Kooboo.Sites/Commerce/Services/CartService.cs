using Dapper;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.ViewModels;
using Kooboo.Sites.Commerce.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class CartService : ServiceBase
    {
        public CartService(RenderContext context) : base(context)
        {
        }

        public void SaveItem(CartItem cartItem)
        {
            using (var con = DbConnection)
            {
                if (con.Exist<CartItem>(cartItem.Id))
                {
                    con.Update(cartItem);
                }
                else
                {
                    con.Insert(cartItem);
                }
            }
        }

        public CartViewModel GetCart(Guid customerId)
        {
            using (var con = DbConnection)
            {
                var list = con.Query(@"
SELECT CI.ProductId,
       CI.SkuId,
       CI.Id,
       CI.Quantity,
       CI.Selected,
       PS.Price,
       P.Title AS ProductName,
       P.Specifications  AS ProductSpecifications,
       PS.Specifications AS ProductSkuSpecifications,
       PT.Specifications AS ProductTypeSpecifications
FROM CartItem CI
         LEFT JOIN ProductSku PS ON CI.SkuId = PS.Id
         LEFT JOIN Product P ON P.Id = PS.ProductId
         LEFT JOIN ProductType PT ON PT.Id = P.TypeId
WHERE Selected = 1
  AND CustomerId = @CustomerId
", new { CustomerId = customerId });

                var items = new List<CartViewModel.CartItemViewModel>();

                foreach (var item in list)
                {
                    items.Add(new CartViewModel.CartItemViewModel
                    {
                        Id = item.Id,
                        Price = (decimal)item.Price,
                        Quantity = item.Quantity,
                        Amount = (decimal)item.Price * item.Quantity,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        SkuId = item.SkuId,
                        Selected = Convert.ToBoolean(item.Selected),
                        Specifications = GetSpecifications(item.ProductSpecifications, item.ProductSkuSpecifications, item.ProductTypeSpecifications)
                    });
                }

                return new CartViewModel
                {
                    Items = items.ToArray(),
                    Amount = items.Where(w => w.Selected).Sum(s=>s.Amount),
                };
            }
        }

        private KeyValuePair<string, string>[] GetSpecifications(string productSpecifications, string productSkuSpecifications, string productTypeSpecifications)
        {
            var productTypes = JsonHelper.Deserialize<ItemDefineViewModel[]>(productTypeSpecifications);
            var productSkus = JsonHelper.Deserialize<KeyValuePair<Guid, Guid>[]>(productSkuSpecifications);
            var products = JsonHelper.Deserialize<KeyValuePair<Guid, string>[]>(productSpecifications);

            var specifications = new List<KeyValuePair<string, string>>();

            foreach (var item in productSkus)
            {
                var productType = productTypes.FirstOrDefault(f => f.Id == item.Key);
                if (productType == null) continue;
                if (productType.Type == ItemDefineViewModel.DefineType.Option)
                {
                    var option = productType.Options.FirstOrDefault(f => f.Key == item.Value);
                    specifications.Add(new KeyValuePair<string, string>(productType.Name, option.Value));
                }
                else if (productType.Type == ItemDefineViewModel.DefineType.Text)
                {
                    var specification = products.FirstOrDefault(f => f.Key == item.Value);
                    specifications.Add(new KeyValuePair<string, string>(productType.Name, specification.Value));
                }
            }

            return specifications.ToArray();
        }
    }
}

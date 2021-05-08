using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.ProductVariant
{
    public class ProductVariantModel
    {
        public ProductVariantModel()
        {

        }

        public ProductVariantModel(Entities.ProductVariant productVariant)
        {
            Id = productVariant.Id;
            Specifications = JsonHelper.Deserialize<KeyValuePair<Guid, Guid>[]>(productVariant.Specifications);
            ProductId = productVariant.ProductId;
            Name = productVariant.Name;
            Price = productVariant.Price;
            Tax = productVariant.Tax;
            Image = productVariant.Image == null ? null : JsonHelper.Deserialize<ProductModel.Image>(productVariant.Image);
            Enable = productVariant.Enable;
        }

        public Guid Id { get; set; }
        public KeyValuePair<Guid, Guid>[] Specifications { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public ProductModel.Image Image { get; set; }
        public bool Enable { get; set; }

        public Entities.ProductVariant ToProductVariant()
        {
            return new Entities.ProductVariant
            {
                Id = Id,
                Enable = Enable,
                Name = Name,
                Price = Price,
                ProductId = ProductId,
                Tax = Tax,
                Image = Image == null ? null : JsonHelper.Serialize(Image),
                Specifications = JsonHelper.Serialize(Specifications)
            };
        }

        public string ToEqualString()
        {
            return string.Join(string.Empty, Specifications.OrderBy(o => o.Key).Select(s => $"{s.Key}{s.Value}"));
        }
    }
}

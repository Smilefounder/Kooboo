using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Sku
{
    public class SkuModel
    {
        public SkuModel()
        {

        }

        public SkuModel(Entities.ProductSku sku)
        {
            Id = sku.Id;
            Specifications = JsonHelper.Deserialize<KeyValuePair<Guid, Guid>[]>(sku.Specifications);
            ProductId = sku.ProductId;
            Name = sku.Name;
            Price = sku.Price;
            Tax = sku.Tax;
            Image = sku.Image == null ? null : JsonHelper.Deserialize<ProductModel.Image>(sku.Image);
            Enable = sku.Enable;
        }

        public Guid Id { get; set; }
        public KeyValuePair<Guid, Guid>[] Specifications { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public ProductModel.Image Image { get; set; }
        public bool Enable { get; set; }

        public Entities.ProductSku ToSku()
        {
            return new Entities.ProductSku
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

using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels.Product
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {

        }

        public ProductViewModel(Entities.Product product, Sku[] skus, Guid[] categories)
        {
            Id = product.Id;
            Title = product.Title;
            Images = JsonHelper.Deserialize<Image[]>(product.Images);
            Description = product.Description;
            Attributes = JsonHelper.Deserialize<KeyValuePair<Guid, string>[]>(product.Attributes);
            Specifications = JsonHelper.Deserialize<KeyValuePair<Guid, string>[]>(product.Specifications);
            TypeId = product.TypeId;
            Enable = product.Enable;
            Skus = skus;
            Categories = categories;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Image[] Images { get; set; }
        public string Description { get; set; }
        public KeyValuePair<Guid, string>[] Attributes { get; set; }
        public KeyValuePair<Guid, string>[] Specifications { get; set; }
        public Guid TypeId { get; set; }
        public bool Enable { get; set; }
        public Sku[] Skus { get; set; }
        public Guid[] Categories { get; set; }

        public class Image
        {
            public Guid Id { get; set; }
            public string Url { get; set; }
            public string Thumbnail { get; set; }
            public bool IsPrimary { get; set; }
        }

        public class Sku
        {
            public Sku()
            {

            }

            public Sku(Entities.Sku sku, int stock)
            {
                Id = sku.Id;
                Specifications = JsonHelper.Deserialize<KeyValuePair<Guid, string>[]>(sku.Specifications);
                ProductId = sku.ProductId;
                Name = sku.Name;
                Price = sku.Price;
                Tax = sku.Tax;
                Thumbnail = sku.Thumbnail;
                Enable = sku.Enable;
                Stock = stock;
            }

            public Guid Id { get; set; }
            public KeyValuePair<Guid, string>[] Specifications { get; set; }
            public Guid ProductId { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public decimal Tax { get; set; }
            public string Thumbnail { get; set; }
            public bool Enable { get; set; }
            public int Stock { get; set; }

            public Entities.Sku ToSku()
            {
                return new Entities.Sku
                {
                    Id = Id,
                    Enable = Enable,
                    Name = Name,
                    Price = Price,
                    ProductId = ProductId,
                    Tax = Tax,
                    Thumbnail = Thumbnail,
                    Specifications = JsonHelper.Serialize(Specifications)
                };
            }
        }

        public Entities.Product ToProduct()
        {
            return new Entities.Product
            {
                Id = Id,
                Description = Description,
                Enable = Enable,
                Images = JsonHelper.Serialize(Images),
                Attributes = JsonHelper.Serialize(Attributes),
                Specifications = JsonHelper.Serialize(Specifications),
                Title = Title,
                TypeId = TypeId
            };
        }
    }
}

using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels.Product
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Image[] Images { get; set; }
        public string Description { get; set; }
        public KeyValuePair<Guid, string>[] Attributes { get; set; }
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
            public Guid Id { get; set; }
            public KeyValuePair<Guid, string>[] Specifications { get; set; }
            public Guid ProductId { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public decimal Tax { get; set; }
            public string Thumbnail { get; set; }
            public bool Enable { get; set; }
            public int Stock { get; set; }
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
                Title = Title,
                TypeId = TypeId
            };
        }

        public Entities.Sku[] ToSkus()
        {
            return Skus.Select(s => new Entities.Sku
            {
                Id = s.Id,
                Enable = s.Enable,
                Name = s.Name,
                Price = s.Price,
                ProductId = s.ProductId,
                Tax = s.Tax,
                Thumbnail = s.Thumbnail,
                Specifications = JsonHelper.Serialize(s.Specifications)
            }).ToArray();
        }
    }
}

using Kooboo.Lib.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Product
{
    public class ProductModel
    {
        public ProductModel()
        {

        }

        public ProductModel(Entities.Product product)
        {
            Id = product.Id;
            Title = product.Title;
            Images = JsonHelper.Deserialize<Image[]>(product.Images);
            Description = product.Description;
            Attributes = JsonHelper.Deserialize<KeyValuePair<Guid, string>[]>(product.Attributes);
            Specifications = JsonHelper.Deserialize<Specification[]>(product.Specifications);
            TypeId = product.TypeId;
            Enable = product.Enable;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Image[] Images { get; set; }
        public string Description { get; set; }
        public KeyValuePair<Guid, string>[] Attributes { get; set; }
        public Specification[] Specifications { get; set; }
        public Guid TypeId { get; set; }
        public bool Enable { get; set; }

        public class Image
        {
            public Guid Id { get; set; }
            public string Url { get; set; }
            public string Thumbnail { get; set; }
            public bool IsPrimary { get; set; }
        }

        public class Specification
        {
            public Guid Id { get; set; }

            public KeyValuePair<Guid, string>[] Options { get; set; }

            public Guid[] Value { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public ItemDefineModel.DefineType Type { get; set; }
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
                TypeId = TypeId,
                CreateTime = DateTime.UtcNow
            };
        }

        public Sku.SkuModel[] ToSkus()
        {
            var result = new List<Sku.SkuModel>();

            void recursion(int i, List<KeyValuePair<Guid, Guid>> specifications)
            {
                if (i > Specifications.Length - 1) return;
                var specification = Specifications[i];

                for (int k = 0; k < specification.Value.Length; k++)
                {
                    var list = specifications.ToList();
                    var value = specification.Value[k];

                    list.Add(new KeyValuePair<Guid, Guid>(specification.Id, value));

                    if (i == Specifications.Length - 1)
                    {
                        result.Add(new Sku.SkuModel
                        {
                            Enable = false,
                            Id = Guid.NewGuid(),
                            Name = string.Empty,
                            ProductId = Id,
                            Image = null,
                            Specifications = list.ToArray()
                        });
                    }
                    else
                    {
                        recursion(i + 1, list);
                    }
                }
            }

            recursion(0, new List<KeyValuePair<Guid, Guid>>());

            if (Specifications.Any() && result.Count == 0)
            {
                throw new Exception("No specification was selected");
            }

            if (Specifications.Length == 0)
            {
                result.Add(new Sku.SkuModel
                {
                    Enable = true,
                    Id = Id,
                    Name = string.Empty,
                    ProductId = Id,
                    Image = null,
                    Specifications = new KeyValuePair<Guid, Guid>[0]
                });
            }

            return result.ToArray();
        }
    }
}

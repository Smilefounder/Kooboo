using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Product
{
    public class ProductTypeModel
    {
        public ProductTypeModel()
        {
        }

        public ProductTypeModel(ProductType entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Attributes = JsonHelper.Deserialize<ItemDefineModel[]>(entity.Attributes);
            Specifications = JsonHelper.Deserialize<ItemDefineModel[]>(entity.Specifications);
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public ItemDefineModel[] Attributes { get; set; }
        public ItemDefineModel[] Specifications { get; set; }

        public ProductType ToEntity()
        {
            return new ProductType
            {
                Id = Id,
                Name = Name,
                Attributes = JsonHelper.Serialize(Attributes),
                Specifications = JsonHelper.Serialize(Specifications)
            };
        }
    }
}

using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels.Product
{
    public class ProductTypeViewModel
    {
        public ProductTypeViewModel()
        {
        }

        public ProductTypeViewModel(ProductType entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Attributes = JsonHelper.Deserialize<ItemDefineViewModel[]>(entity.Attributes);
            Specifications = JsonHelper.Deserialize<ItemDefineViewModel[]>(entity.Specifications);
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public ItemDefineViewModel[] Attributes { get; set; }
        public ItemDefineViewModel[] Specifications { get; set; }

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

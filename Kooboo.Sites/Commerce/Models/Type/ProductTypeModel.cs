using Kooboo.Data.Attributes;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Type
{
    public class ProductTypeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ItemDefineModel[] Attributes { get; set; }
        public ItemDefineModel[] Specifications { get; set; }

        [KIgnore]
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

        [KIgnore]
        public void Valid()
        {
            Validator.NotEmpty(Id, "Id");
            Validator.NotEmpty(Name, "Name");
            Validator.StringRange(Name, "Name", 2, 31);
            Validator.NotNull(Attributes, "Attributes");
            Validator.NotNull(Specifications, "Specifications");

            foreach (var item in Attributes)
            {
                Validator.NotEmpty(item.Id, "Attribute id");
                Validator.NotEmpty(item.Name, "Attribute name");
                Validator.StringRange(item.Name, "Name", 2, 31);

                if (item.Type == ItemDefineModel.DefineType.Option)
                {
                    Validator.NotNull(item.Options, "Attribute options");
                    Validator.NotEmpty(item.Options.Length, "Attribute options");
                }

            }

            foreach (var item in Specifications)
            {
                Validator.NotEmpty(item.Id, "Specification id");
                Validator.NotEmpty(item.Name, "Specification name");
                Validator.StringRange(item.Name, "Name", 2, 31);

                if (item.Type == ItemDefineModel.DefineType.Option)
                {
                    Validator.NotNull(item.Options, "Specification options");
                    Validator.NotEmpty(item.Options.Length, "Specification options");
                }
            }
        }
    }
}

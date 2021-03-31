using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Category
{
    public class EditCategoryModel : CategoryModel
    {
        public EditCategoryModel() { }

        public EditCategoryModel(Entities.Category category, Guid[] products)
        {
            Id = category.Id;
            Name = category.Name;
            Type = category.Type;
            Rule = JsonHelper.Deserialize<MatchRule.Rule>(category.Rule);
            Products = products;
        }

        public Guid[] Products { get; set; }

        public Entities.Category ToCategory()
        {
            return new Entities.Category
            {
                Id = Id,
                Name = Name,
                Rule = JsonHelper.Serialize(Rule),
                Type = Type
            };
        }
    }
}

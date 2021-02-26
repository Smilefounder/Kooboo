using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels.Category
{
    public class EditCategoryViewModel : CategoryViewModel
    {
        public EditCategoryViewModel() { }

        public EditCategoryViewModel(Entities.Category category, Guid[] products)
        {
            Id = category.Id;
            Name = category.Name;
            Type = category.Type;
            Rules = JsonHelper.Deserialize<Rule[]>(category.Rules);
            Products = products;
        }

        public Guid[] Products { get; set; }

        public Entities.Category ToCategory()
        {
            return new Entities.Category
            {
                Id = Id,
                Name = Name,
                Rules = JsonHelper.Serialize(Rules),
                Type = Type
            };
        }
    }
}

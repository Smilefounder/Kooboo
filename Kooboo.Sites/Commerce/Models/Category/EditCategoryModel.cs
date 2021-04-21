using System;

namespace Kooboo.Sites.Commerce.Models.Category
{
    public class EditCategoryModel : CategoryModel
    {
        public EditCategoryModel() { }

        public EditCategoryModel(CategoryModel category, Guid[] products)
        {
            Helpers.FillBase(category, this);
            Products = products;
        }

        public Guid[] Products { get; set; }
    }
}

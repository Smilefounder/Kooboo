using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Category
{
    public class CategoryListModel : CategoryModel
    {
        public CategoryListModel(CategoryModel category, int productCount)
        {
            Helpers.FillBase(category, this);
            ProductCount = productCount;
        }

        public int ProductCount { get; set; }
    }
}

using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Category
{
    public class CategoryListModel : CategoryModel
    {
        public CategoryListModel()
        {

        }

        public CategoryListModel(Entities.Category category, int productCount)
        {
            Id = category.Id;
            Name = category.Name;
            Type = category.Type;
            Rule = JsonHelper.Deserialize<MatchRule.Rule>(category.Rule);
            ProductCount = productCount;
            Enable = category.Enable;
        }

        public int ProductCount { get; set; }
    }
}

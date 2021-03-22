using Kooboo.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels.Category
{
    public class CategoryListViewModel : CategoryViewModel
    {
        public CategoryListViewModel()
        {

        }

        public CategoryListViewModel(Entities.Category category, int productCount)
        {
            Id = category.Id;
            Name = category.Name;
            Type = category.Type;
            Rule = JsonHelper.Deserialize<MatchRule.Rule>(category.Rule);
            ProductCount = productCount;
        }

        public int ProductCount { get; set; }
    }
}

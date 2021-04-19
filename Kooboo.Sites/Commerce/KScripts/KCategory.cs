using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Models.Category;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Kooboo.Sites.Commerce.KScripts
{
    public class KCategory
    {

        readonly Lazy<CategoryService> _categoryService;

        public KCategory(RenderContext context)
        {
            _categoryService = new Lazy<CategoryService>(() => new CategoryService(context), true);
        }

        [Description("Get catrgory list")]
        [KDefineType(Return = typeof(CategoryListModel[]))]
        public object List()
        {
            var list = _categoryService.Value.List();
            return Helpers.ToKscriptModel(list);
        }
    }
}

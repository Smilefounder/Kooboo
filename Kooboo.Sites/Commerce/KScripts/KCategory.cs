using Jint.Native;
using Jint.Native.Array;
using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Data.Interface;
using Kooboo.Lib.Reflection;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.KScripts
{
    public class KCategory
    {
        readonly Lazy<CategoryService> _productCategoryService;
        readonly RenderContext _context;

        public KCategory(RenderContext context)
        {
            _context = context;
            _productCategoryService = new Lazy<CategoryService>(() => new CategoryService(context), true);
        }

        [KDefineType(Return = typeof(Category[]))]
        public object List()
        {
            var engine = Kooboo.Sites.Scripting.Manager.GetJsEngine(_context);
            return _productCategoryService.Value.List().Select(s => JsValue.FromObject(engine, s)).ToArray();
        }

        //[KDefineType(Params = new[] { typeof(Category[]) })]
        //public void Save(IDictionary<string, object>[] productCategories)
        //{
        //    var list = productCategories.Select(s => TypeHelper.ToObject<Category>(s)).ToArray();
        //    _productCategoryService.Value.Save(list);
        //}
    }
}

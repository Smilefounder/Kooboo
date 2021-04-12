using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Models.Product;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Kooboo.Sites.Commerce.KScripts
{
    public class KProduct
    {
        readonly Lazy<ProductService> _productService;

        public KProduct(RenderContext context)
        {
            _productService = new Lazy<ProductService>(() => new ProductService(context), true);
        }


        [Description(@"

")]
        [KDefineType(Params = new[] { typeof(ProductModel) })]
        public void Post(object obj)
        {
            var model = Helpers.FromKscriptModel<ProductModel>(obj);
            _productService.Value.Save(model);
        }
    }
}

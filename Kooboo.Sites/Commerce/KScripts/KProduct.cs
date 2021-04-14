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
        readonly Lazy<ProductSkuService> _productSkuService;
        readonly RenderContext _context;

        public KProduct(RenderContext context)
        {
            _context = context;
            _productService = new Lazy<ProductService>(() => new ProductService(context), true);
            _productSkuService = new Lazy<ProductSkuService>(() => new ProductSkuService(context), true);
        }


        [Description(@"

")]
        [KDefineType(Params = new[] { typeof(ProductModel) })]
        public void Post(object obj)
        {
            var model = Helpers.FromKscriptModel<ProductModel>(obj);

            _context.CreateCommerceDbConnection().ExecuteTask(con =>
            {
                _productService.Value.Save(model, con);
                _productSkuService.Value.Save(model.Id, model.ToSkus(), con);
            }, true);
        }
    }
}

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
k.commerce.product.post({
    id: k.security.newGuid(),
    title: 'product1',
    attributes: [],
    typeId: 'b2b0e76e-74ba-4990-9c25-1330c90e7ca4',
    enable: true,
    description:'',
    images:[],
    specifications: [
        {
            id: '1c0837d6-9928-41b7-b136-0a0f312f12a9',
            options: [
                {
                    'key': 'cddf9035-4c79-49ee-a676-b555a98c0175',
                    'value': 'White'
                },
                {
                    'key': '21970d5d-716d-4395-8de2-ca85687e184c',
                    'value': 'Black'
                }
            ]
        }
    ]
})
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

        [Description(@"
k.commerce.product.get('b2b0e76e-74ba-4990-9c25-1330c90e7ca4')
")]
        public object Get(string id)
        {
            return _productService.Value.GetForKscript(Guid.Parse(id));
        }

        //public void GetList()
        //{

        //}
    }
}

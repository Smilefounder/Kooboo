using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Sites.Commerce.Models.Type;
using Kooboo.Sites.Commerce.Services;
using System;
using System.ComponentModel;

namespace Kooboo.Sites.Commerce.KScripts
{
    public class KType
    {
        readonly Lazy<ProductTypeService> _productTypeService;

        public KType(RenderContext context)
        {
            var commerce = SiteCommerce.Get(context.WebSite);
            _productTypeService = new Lazy<ProductTypeService>(() => new ProductTypeService(commerce), true);
        }

        [Description("Get product type list")]
        [KDefineType(Return = typeof(ProductTypeDetailModel[]))]
        public object List()
        {
            var list = _productTypeService.Value.List();
            return Helpers.ToKscriptModel(list);
        }


        [Description(@"
k.commerce.type.post({
    id: k.security.newGuid(),
    name: 'type1',
    attributes: [
        {
            id: k.security.newGuid(),
            name: 'attribute1',
            type: 'Text' //enum: Text Option
        }
    ],
    specifications: [
        {
           id: k.security.newGuid(),
            name: 'specification1',
            type: 'Option', //enum: Text Option
            options: [{
                key: k.security.newGuid(),
                value: 'options1'
            }]
        }
    ],
})
")]
        [KDefineType(Params = new[] { typeof(ProductTypeModel) })]
        public void Post(object obj)
        {
            var model = Helpers.FromKscriptModel<ProductTypeModel>(obj);
            _productTypeService.Value.Save(model);
        }

        [Description(@"
Delete product type by ids

k.commerce.type.delete([
    '9e7da080-0479-4f4b-927a-7f544cac6e53',
    'fc7953ee-46b6-4615-966b-e1d7c9431095',
    'fee7a78e-23ed-417c-b2a5-8ece9b58bbf3'
])
")]
        [KDefineType(Params = new[] { typeof(string[]) })]
        public void Delete(object obj)
        {
            var model = Helpers.FromKscriptModel<Guid[]>(obj);
            _productTypeService.Value.Delete(model);
        }

        [KDefineType(Params = new[] { typeof(string[]) })]
        public object KeyValue()
        {
            var list = _productTypeService.Value.KeyValue();
            return Helpers.ToKscriptModel(list);
        }
    }
}

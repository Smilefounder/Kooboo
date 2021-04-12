using Kooboo.Data.Attributes;
using Kooboo.Data.Context;
using Kooboo.Data.Interface;
using Kooboo.Sites.Commerce.KScripts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce
{
    public class KCommerce : IkScript
    {
        [KIgnore]
        public string Name => "commerce";

        [KIgnore]
        public RenderContext context { get; set; }

        readonly Lazy<KType> _type;
        readonly Lazy<KProduct> _product;

        public KType Type => _type.Value;
        public KProduct Product => _product.Value;

        public KCommerce()
        {
            _type = new Lazy<KType>(() => new KType(context), true);
            _product = new Lazy<KProduct>(() => new KProduct(context), true);
        }
    }
}

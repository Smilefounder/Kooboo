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

        readonly Lazy<KCategory> _category;

        public KCategory Category => _category.Value;

        public KCommerce()
        {
            _category = new Lazy<KCategory>(() => new KCategory(context), true);
        }
    }
}

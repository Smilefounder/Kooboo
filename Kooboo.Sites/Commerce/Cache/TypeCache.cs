using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.Models.Type;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Cache
{
    public class TypeCache : CacheBase<ProductTypeModel[]>
    {
        public TypeCache(SiteCommerce commerce) : base(commerce)
        {
            var productTypeService = commerce.Service<ProductTypeService>();
            productTypeService.OnChanged += _ => Clear();
            productTypeService.OnDeleted += _ => Clear();
        }

        protected override ProductTypeModel[] OnGet()
        {
            return Commerce.CreateDbConnection().ExecuteTask(con =>
            {
                return con.GetList<ProductType>().Select(s => new ProductTypeModel(s)).ToArray();
            });
        }
    }
}

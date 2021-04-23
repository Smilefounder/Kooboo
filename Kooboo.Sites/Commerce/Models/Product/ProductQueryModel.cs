using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Product
{
    public class ProductQueryModel : PagingQueryModel
    {
        public Guid? TypeId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}

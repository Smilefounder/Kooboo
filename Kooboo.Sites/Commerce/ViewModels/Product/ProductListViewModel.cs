using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels.Product
{
    public class ProductListViewModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public long Stock { get; set; }
        public long Sales { get; set; }
        public bool Enable { get; set; }
        public string Images { get; set; }
    }
}

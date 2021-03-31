using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models
{
    public class PagingQueryModel
    {
        //start from 1
        public long Index { get; set; }
        public long Size { get; set; }
    }
}

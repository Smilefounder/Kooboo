using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.ViewModels
{
    public class PagingQueryViewModel
    {
        //start from 1
        public long Index { get; set; }
        public long Size { get; set; }
    }
}

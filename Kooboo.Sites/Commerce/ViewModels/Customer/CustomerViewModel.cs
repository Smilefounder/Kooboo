using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;

namespace Kooboo.Sites.Commerce.ViewModels
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {

        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public int Cart { get; set; }
    }
}

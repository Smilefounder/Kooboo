using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;

namespace Kooboo.Sites.Commerce.Models
{
    public class CustomerListModel
    {
        public CustomerListModel()
        {

        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreateTime { get; set; }
        public int Cart { get; set; }
    }
}

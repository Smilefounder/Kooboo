using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data.Context;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.Promotion;

namespace Kooboo.Sites.Ecommerce.ViewModel
{
    public class OrderEditRequestModel
    {

        public Guid Id { get; set; }

        public OrderAddress OrderAddress { get; set; }

        public string logisticsCompany { get; set; }

        public string logisticsNumber { get; set; }
    }
}

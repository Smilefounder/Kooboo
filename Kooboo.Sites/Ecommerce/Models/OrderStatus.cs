using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Ecommerce.Models
{
    public enum OrderStatus
    {
        Created,
        Paid,
        Shipping,
        Finished,
        Cancel
    }
}

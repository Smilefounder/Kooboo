using Kooboo.Api;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Order;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class OrderApi : CommerceApi
    {
        public override string ModelName => "Order";

        public void Create(CreateOrderModel model, ApiCall apiCall)
        {
            GetService<OrderService>(apiCall).Create(model);
        }

        public OrderDetailModel Get(Guid id, ApiCall apiCall)
        {
            return GetService<OrderService>(apiCall).Get(id);
        }

        public OrderPreviewModel Preview(Guid id, ApiCall apiCall)
        {
            return GetService<OrderService>(apiCall).Preview(id);
        }

        public PagedListModel<OrderListModel> List(PagingQueryModel model, ApiCall apiCall)
        {
            return GetService<OrderService>(apiCall).Query(model);
        }
    }
}

using Kooboo.Api;
using Kooboo.Sites.Commerce.Models;
using Kooboo.Sites.Commerce.Models.Order;
using Kooboo.Sites.Commerce.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Web.Api.Implementation.Commerce
{
    public class OrderApi : IApi
    {
        public string ModelName => "Order";

        public bool RequireSite => true;

        public bool RequireUser => false;

        public void Create(CreateOrderModel model, ApiCall apiCall)
        {
            new OrderService(apiCall.Context).Create(model);
        }

        public OrderDetailModel Get(Guid id, ApiCall apiCall)
        {
            return new OrderService(apiCall.Context).Get(id);
        }

        public OrderPreviewModel Preview(Guid id, ApiCall apiCall)
        {
            return new OrderService(apiCall.Context).Preview(id);
        }

        public PagedListModel<OrderListModel> List(PagingQueryModel model, ApiCall apiCall)
        {
            return new OrderService(apiCall.Context).Query(model);
        }
    }
}

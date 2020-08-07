using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data.Context;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Ecommerce.Promotion;

namespace Kooboo.Sites.Ecommerce.ViewModel
{
    public class OrderViewModel
    {
        Order order { get; set; }

        RenderContext context { get; set; }

        public OrderViewModel(Order order, RenderContext context)
        {
            this.order = order;
            this.context = context;
        }

        public Guid Id => order.Id;

        public Guid CustomerId => order.CustomerId;

        public Guid PaymentRequestId => order.PaymentRequestId;

        public Guid AddressId => order.AddressId;

        public DateTime CreateDate => order.CreateDate;

        public string Status => Enum.GetName(typeof(OrderStatus), order.Status);

        public decimal ItemTotal => order.ItemTotal;

        public decimal TotalAmount => order.TotalAmount;

        public OrderLineViewModel[] Items
        {
            get
            {
                List<OrderLineViewModel> result = new List<OrderLineViewModel>();
                foreach (var item in this.order.Items)
                {
                    OrderLineViewModel model = new OrderLineViewModel(item, this.context);
                    result.Add(model);
                }
                return result.ToArray();
            }
        }

        public CustomerViewModel Customer
        {
            get
            {
                var customer = ServiceProvider.Customer(this.context).Get(this.CustomerId);
                if (customer != null)
                {
                    return new CustomerViewModel(customer, context);
                }
                return null;
            }
        }

        public decimal ShippingCost => order.ShippingCost;

        public OrderAddress OrderAddress => order.OrderAddress;

        public string LogisticsCompany => order.LogisticsCompany;

        public string LogisticsNumber => order.LogisticsNumber;

        public Discount Discount => order.Discount;
    }

    public class OrderLineViewModel
    {
        RenderContext context { get; set; }

        public OrderLineViewModel(OrderLine item, RenderContext context)
        {
            this.ProductVariantId = item.ProductVariantId;
            this.ProductId = item.ProductId;
            this.Quantity = item.Quantity;
            this.UnitPrice = item.UnitPrice;
            this.Discount = item.Discount;
            this.ItemTotal = item.ItemTotal;
            this.context = context;
        }

        public ProductViewModel Product
        {
            get
            {
                var product = ServiceProvider.Product(this.context).Get(this.ProductId);
                if (product != null)
                {

                    var type = ServiceProvider.ProductType(this.context).Get(product.ProductTypeId);

                    if (type != null)
                    {
                        return new ProductViewModel(product, this.context, type.Properties);
                    }
                }
                return null;
            }
        }

        public ProductVariantsViewModel Variants
        {
            get
            {
                var varants = ServiceProvider.ProductVariants(this.context).Get(this.ProductVariantId);
                if (varants != null)
                {
                    return new ProductVariantsViewModel(varants);
                }
                return null;
            }
        }

        public Guid ProductVariantId { get; set; }

        /// <summary>
        /// For redundancy
        /// </summary>
        public Guid ProductId { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal UnitPrice { get; set; }

        public Discount Discount { get; set; }

        public decimal ItemTotal { get; set; }
    }

}

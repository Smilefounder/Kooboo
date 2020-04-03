using Kooboo.Data.Context;
using Kooboo.Data.Interface;
using Kooboo.Sites.Ecommerce;
using Kooboo.Sites.Ecommerce.KScript;
using KScript.Ecommerce;
using System;
using System.Collections.Generic;
using System.Text;

namespace KScript
{
    public class Kcommerce : IkScript
    {
        public string Name => "commerce";

        public RenderContext context { get; set; }

        public CommerceContext commerceContext { get; set; }

        private KCategory _category;
        public KCategory Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new KCategory(this.context);
                }
                return _category;
            }
        }

        private KProduct _product;
        public KProduct Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new KProduct(this.context);
                }
                return _product;
            }
        }

        private KOrder _order;
        public KOrder Order
        {
            get
            {
                if (_order == null)
                {
                    _order = new KOrder(this.context);
                }
                return _order;
            }
        }


        private KCart _shoppingcart;
        public KCart ShoppingCart
        {
            get
            {
                if (_shoppingcart == null)
                {
                    _shoppingcart = new KCart(this.context);
                }
                return _shoppingcart;
            }
        }

        private KCustomerAddress _customerAddress;
        public KCustomerAddress CustomerAddress
        {
            get
            {
                if (_customerAddress == null)
                {
                    _customerAddress = new KCustomerAddress(this.context);
                }
                return _customerAddress;
            }
        }

    }
}

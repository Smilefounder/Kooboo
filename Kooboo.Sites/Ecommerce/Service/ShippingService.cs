using Kooboo.Data.Context;
using Kooboo.Sites.Ecommerce.Models;
using Kooboo.Sites.Logistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.Sites.Ecommerce.Service
{
    public class ShippingService : IEcommerceService
    {
        public RenderContext Context { get; set; }
        public CommerceContext CommerceContext { get; set; }

        public long Priority => 1;

        public decimal CalculateCost(Cart cart)
        {
            return (decimal)2.95;
        }

        private List<LogisticsCompany> _logistics;
        public List<LogisticsCompany> GetAllLogistics()
        {
            if (_logistics == null)
            {
                _logistics = new List<LogisticsCompany>();
                var alltypes = Lib.Reflection.AssemblyLoader.LoadTypeByInterface(typeof(ILogisticsMethod));
                foreach (var item in alltypes)
                {
                    var instance = Activator.CreateInstance(item) as ILogisticsMethod;
                    if (instance != null)
                    {
                        _logistics.Add(new LogisticsCompany
                        {
                            DisplayName = instance.DisplayName,
                            Name = instance.Name
                        });
                    }
                }
                _logistics = _logistics.OrderBy(o => o.DisplayName).ToList();
            }
            return _logistics;
        }
    }
}

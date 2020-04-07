using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics
{
    public class LogisticsContainer
    {
        private static object _locker = new object();

        private static List<ILogisticsMethod> _paymentmethods;

        public static List<ILogisticsMethod> PaymentMethods
        {
            get
            {
                if (_paymentmethods == null)
                {
                    _paymentmethods = Lib.IOC.Service.GetInstances<ILogisticsMethod>();
                }
                return _paymentmethods;
            }
        }
    }
}

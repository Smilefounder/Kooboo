using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics
{
    public class LogisticsContainer
    {
        private static object _locker = new object();

        private static List<ILogisticsMethod> _logisticsMethods;

        public static List<ILogisticsMethod> LogisticsMethods
        {
            get
            {
                if (_logisticsMethods == null)
                {
                    _logisticsMethods = Lib.IOC.Service.GetInstances<ILogisticsMethod>();
                }
                return _logisticsMethods;
            }
        }
    }
}

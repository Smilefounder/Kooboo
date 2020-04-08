using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Kooboo.Data.Context;
using Kooboo.Sites.Logistics.Models;

namespace Kooboo.Sites.Logistics
{
    public interface ILogisticsMethod<T> : ILogisticsMethod where T : ILogisticsSetting
    {
        [Description("Account settig to be used for this payment method")]
        T Setting { get; set; }
    }

    public interface ILogisticsMethod
    {
        [Description("The name that can be used for k.payment.{name}")]
        string Name { get; }

        string DisplayName { get; }

        RenderContext Context { get; set; }
        
        ILogisticsResponse CreateOrder(LogisticsRequest request);

        LogisticsStatusResponse checkStatus(LogisticsRequest request);
    }
}

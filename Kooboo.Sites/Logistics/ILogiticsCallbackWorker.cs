using System;
using System.Collections.Generic;
using System.Text;
using Kooboo.Data.Context;
using Kooboo.Sites.Logistics.Models;

namespace Kooboo.Sites.Logistics
{
    public interface ILogiticsCallbackWorker
    {
        void Process(LogisticsCallback callback, RenderContext context);
    }
}

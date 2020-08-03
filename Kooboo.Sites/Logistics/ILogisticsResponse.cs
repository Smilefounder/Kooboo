using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Kooboo.Sites.Logistics
{
    public interface ILogisticsResponse
    {
        [Description(@"Kooboo create order request reference id, used to query logistics status at: /_api/logistics/checkstatus?id={requestId}")]
        Guid requestId { get; set; }

        [Description("Set value in case the Logistics Method return an reference id itself")]
        string logisticsMethodReferenceId { get; set; }
    }
}

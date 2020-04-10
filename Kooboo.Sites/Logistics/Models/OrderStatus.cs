using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Models
{
    public enum OrderStatus
    {
        Init,
        Got,//收件
        Arrival,//发件
        Send,//已到达
        Scan,//派件扫描
        Signed,//签收
        SignFailed,//异常签收
        ThirdPartSign,//第三方签收
        Failed,//退回件
        FailedSigned,//退回件
        Problem //	问题件
    }
}

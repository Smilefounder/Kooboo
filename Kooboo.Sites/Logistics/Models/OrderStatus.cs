using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Models
{
    public enum OrderStatus
    {
        GOT,//收件
        ARRIVAL,//发件
        SENT_CITY,//已到达
        SENT_SCAN,//派件扫描
        SIGNED,//签收
        SIGNFAIL,//异常签收
        DSQS,//第三方签收
        FAILED,//退回件
        PROBLEM//	问题件
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.best.Model
{
	public class KdCreateOrderNotifyRsp 
	{
		public bool result { get; set; }
		public string remark { get; set; }
		public string errorCode { get; set; }
		public string errorDescription { get; set; }
	}
}

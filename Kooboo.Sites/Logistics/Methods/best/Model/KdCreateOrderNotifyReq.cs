using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.best.Model
{
	public class KdCreateOrderNotifyReq 
	{
		public string txLogisticID { get; set; }
		public string tradeNo { get; set; }
		public string mailNo { get; set; }
		public string orderType { get; set; }
		public string serviceType { get; set; }
		public string orderFlag { get; set; }
		public Sender sender { get; set; }
		public Receiver receiver { get; set; }
	}

	public class Sender
	{
		public string name { get; set; }
		public string postCode { get; set; }
		public string phone { get; set; }
		public string prov { get; set; }
		public string city { get; set; }
		public string county { get; set; }
		public string address { get; set; }
		public string country { get; set; }
	}

	public class Receiver
	{
		public string name { get; set; }
		public string phone { get; set; }
		public string prov { get; set; }
		public string city { get; set; }
		public string county { get; set; }
		public string address { get; set; }
		public string country { get; set; }
	}
}

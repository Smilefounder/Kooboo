using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Logistics.Methods.best.Model
{
	public class KdTraceQueryRsp 
	{
		public bool result { get; set; }
		public string remark { get; set; }
		public string errorCode { get; set; }
		public string errorDescription { get; set; }
		public List<TraceLogs> traceLogs { get; set; }
	}

	public class TraceLogs
	{
		public Problems problems { get; set; }
		public bool check { get; set; }
		public string mailNo { get; set; }
		public Traces traces { get; set; }
	}

	public class Problems
	{
		public List<Problem> problem { get; set; }
	}


	public class Problem
	{
		public long? seqNum { get; set; }
		public string problemType { get; set; }
		public string registerMan { get; set; }
		public string registerDate { get; set; }
		public string registerSite { get; set; }
		public string problemCause { get; set; }
		public string noticeSite { get; set; }
		public string replyMan { get; set; }
		public string replyDate { get; set; }
		public string replyContent { get; set; }
	}

	public class Traces
	{
		public List<Trace> trace { get; set; }
	}

	public class Trace
	{
		public string acceptTime { get; set; }
		public string acceptAddress { get; set; }
		public string scanType { get; set; }
		public string remark { get; set; }
	}
}

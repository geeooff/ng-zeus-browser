using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeusBrowser.Server.Core;

namespace ZeusBrowser.Server.Models.Browse
{
    public class OrderedResult : PathResult
    {
		public GroupBy GroupBy { get; set; }
		public OrderBy OrderBy { get; set; }
	}
}

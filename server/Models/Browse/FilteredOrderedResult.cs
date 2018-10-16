using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeusBrowser.Server.Core;

namespace ZeusBrowser.Server.Models.Browse
{
    public class FilteredOrderedResult : OrderedResult
	{
		public MediaTypeType? MediaType { get; set; }
	}
}

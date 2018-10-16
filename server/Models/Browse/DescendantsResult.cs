using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Models.Browse
{
	public class DescendantsResult : FilteredOrderedResult
	{
		public List<Fso> Descendants { get; set; }
	}
}

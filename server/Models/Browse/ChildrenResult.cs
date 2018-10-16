using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Models.Browse
{
	public class ChildrenResult : OrderedResult
	{
		public List<Fso> Children { get; set; }
	}
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Models.Browse
{
    public class SiblingsResult : OrderedResult
    {
		public List<PathResult> Siblings { get; set; }
		public Fso Previous { get; set; }
		public Fso Next { get; set; }
	}
}

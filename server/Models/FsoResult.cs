using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Models
{
	/// <summary>
	/// Full File System Object API action result
	/// </summary>
    public class FsoResult
	{
		/// <summary>
		/// File System Object
		/// </summary>
		public Fso Fso { get; set; }
	}
}

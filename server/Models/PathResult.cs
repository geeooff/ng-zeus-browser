using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Models
{
	/// <summary>
	/// Simplified File System Object action result as URI-abspath only
	/// </summary>
	public class PathResult
	{
		/// <summary>
		/// File System Object URI-abspath
		/// </summary>
		public string Path { get; set; }
	}
}

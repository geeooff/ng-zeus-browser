using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Configuration
{
	public class AppSettings
	{
		public string RootName { get; set; }
		public string PhysicalPath { get; set; }
		public Uri ExternalPath { get; set; }
		public SecureLinks SecureLinks { get; set; }
		public string[] SkipFolders { get; set; }
		public TimeSpan CacheExpiration { get; set; }
		public string MediaInfoBin { get; set; }
	}
}

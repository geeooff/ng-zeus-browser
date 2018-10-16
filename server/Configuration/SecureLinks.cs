using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Configuration
{
    public class SecureLinks
    {
		public Uri Path { get; set; }
		public TimeSpan Expiration { get; set; }
		public string Format { get; set; }
		public SecureLinksArgs Args { get; set; }
	}
}

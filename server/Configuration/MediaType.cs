using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Configuration
{
    public class MediaType
    {
		public Dictionary<string,string> MimeTypes { get; set; }
		public Dictionary<string,string> PlayerMimeTypes { get; set; }
	}
}

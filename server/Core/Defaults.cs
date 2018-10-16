using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Core
{
    public static class Defaults
    {
		public const string AntiforgeryHeaderName = "X-Zeus-Lightning";
		public const string AntiforgeryCookieName = "Zeus-Lightning";
		public const string Path = "/";
		public const GroupBy GroupBy = Core.GroupBy.FileSystemInfoType;
		public const OrderBy OrderBy = Core.OrderBy.None;
		public const int StreamWriterBufferSize = 512 * 1024;
		public const string InternalUriScheme = "zb";
		public const string InternetUriHost = "zeus-browser";
		public const int MediaInfoProcessTimeout = 30 * 1000;
	}
}

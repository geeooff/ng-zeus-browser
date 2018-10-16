using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Core
{
    public class MediaInfoTimeoutException : MediaInfoException
	{
		public MediaInfoTimeoutException(string message) : base(message) { }
		public MediaInfoTimeoutException(string message, Exception innerException) : base(message, innerException) { }
	}
}

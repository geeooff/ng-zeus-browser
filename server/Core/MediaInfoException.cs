using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Core
{
    public class MediaInfoException : ApplicationException
    {
		public MediaInfoException() { }
		public MediaInfoException(string message) : base(message) { }
		public MediaInfoException(string message, Exception innerException) : base(message, innerException) { }
	}
}

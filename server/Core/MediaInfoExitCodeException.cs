using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Core
{
    public class MediaInfoExitCodeException : MediaInfoException
	{
		public MediaInfoExitCodeException(string message) : base(message) { }
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Core
{
	public class UnknownFileSystemInfo : FileSystemInfo
	{
		private string _name;

		public UnknownFileSystemInfo(string name, string originalPath)
		{
			_name = name;
			OriginalPath = originalPath;
		}

		public override bool Exists => false;

		public override string Name => _name;

		public override void Delete()
		{
			throw new NotSupportedException("Can't delete an unknown file or directory");
		}
	}
}

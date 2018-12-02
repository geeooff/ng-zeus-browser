using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ZeusBrowser.Server.Core
{
	/// <summary>
	/// Wrapper around IFileInfo representing the file system root
	/// </summary>
	public class RootFileInfo : IRootFileInfo
	{
		private readonly IFileInfo _fileInfo;

		public RootFileInfo(IFileInfo fileInfo)
		{
			_fileInfo = fileInfo;
		}

		public bool Exists => _fileInfo.Exists;

		public long Length => _fileInfo.Length;

		public string PhysicalPath => _fileInfo.PhysicalPath;

		public string Name => _fileInfo.Name;

		public DateTimeOffset LastModified => _fileInfo.LastModified;

		public bool IsDirectory => _fileInfo.IsDirectory;

		public Stream CreateReadStream() => _fileInfo.CreateReadStream();
	}
}

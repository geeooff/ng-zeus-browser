using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZeusBrowser.Server.Services;
using ZeusBrowser.Server.Core;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.AspNetCore.WebUtilities;

namespace ZeusBrowser.Server.Models
{
	[JsonObject(MemberSerialization.OptOut)]
	public class Fso
	{
		public Fso(
			FileSystemInfo fsi,
			string name,
			Uri uri,
			MediaType mediaType,
			DateTime instanceCreated)
		{
			FileSystemInfo = fsi;
			Name = name;
			Uri = uri;
			MediaType = mediaType;
			InstanceCreated = instanceCreated;
		}

		internal FileSystemInfo FileSystemInfo { get; private set; }

		public string Name { get; private set; }

		internal Uri Uri { get; private set; }

		public string Path => Uri.AbsolutePath;

		public MediaType MediaType { get; private set; }

		public bool Exists => FileSystemInfo.Exists;

		public bool IsDir => FileSystemInfo is DirectoryInfo;

		public bool IsFile => FileSystemInfo is FileInfo;

		public long FileSize => FileSystemInfo is FileInfo fi ? fi.Length : 0L;

		public DateTime? Created => Exists ? FileSystemInfo.CreationTime : (DateTime?)null;

		public DateTime? Modified => Exists ? FileSystemInfo.LastWriteTime : (DateTime?)null;

		public string FileExtension => IsFile ? FileSystemInfo.Extension.TrimStart('.') : null;

		public DateTime InstanceCreated { get; private set; }

		public TimeSpan InstanceAged => DateTime.Now - InstanceCreated;
	}
}
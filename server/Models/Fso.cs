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
using Microsoft.Extensions.FileProviders;

namespace ZeusBrowser.Server.Models
{
	[JsonObject(MemberSerialization.OptOut)]
	public class Fso
	{
		public Fso(
			IFileInfo fi,
			string name,
			Uri uri,
			MediaType mediaType,
			DateTime instanceCreated)
		{
			FileInfo = fi;
			Name = name;
			Uri = uri;
			MediaType = mediaType;
			InstanceCreated = instanceCreated;
		}

		internal IFileInfo FileInfo { get; private set; }

		public string Name { get; private set; }

		internal Uri Uri { get; private set; }

		public string Path => Uri.AbsolutePath;

		public MediaType MediaType { get; private set; }

		public bool Exists => FileInfo.Exists;

		public bool IsDir => FileInfo.IsDirectory;

		public bool IsFile => !IsDir;

		public long FileSize => IsFile ? FileInfo.Length : 0L;

		// Creation date is not exposed by IFileInfo or IFileProvider
		//public DateTime? Created => Exists ? FileInfo.CreationTime : (DateTime?)null;

		public DateTimeOffset? Modified => Exists ? FileInfo.LastModified : (DateTimeOffset?)null;

		public string FileExtension => IsFile ? System.IO.Path.GetExtension(FileInfo.Name).TrimStart('.') : null;

		public DateTime InstanceCreated { get; private set; }

		public TimeSpan InstanceAged => DateTime.Now - InstanceCreated;
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZeusBrowser.Server.Configuration;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;
using ZeusBrowser.Server.LoggerExtensions;
using Microsoft.Extensions.FileProviders;

namespace ZeusBrowser.Server.Services
{
	public class FsoFactoryService
	{
		private readonly ILogger<FsoFactoryService> _logger;
		private readonly AppSettings _options;
		private readonly IFileProvider _fileProvider;
		private readonly FsoUriService _uriService;
		private readonly FsoMediaTypeFactoryService _mediaTypeFactoryService;

		public FsoFactoryService(
			ILogger<FsoFactoryService> logger,
			IOptions<AppSettings> optionsAccessor,
			IFileProvider fileProvider,
			FsoUriService uriService,
			FsoMediaTypeFactoryService mediaTypeFactoryService)
		{
			_logger = logger;
			_options = optionsAccessor.Value;
			_fileProvider = fileProvider;
			_uriService = uriService;
			_mediaTypeFactoryService = mediaTypeFactoryService;

			// TODO Replace DirectoryInfo and FileInfo by secured IFileInfo references
			// root object
			Root = Create(
				new DirectoryInfo(_options.PhysicalPath),
				_options.RootName,
				_uriService.Get("/")
			);

			_logger.RootCreated(Root);
		}

		public Fso Root { get; }

		public Fso Create(Uri uri)
		{
			List<string> paths = _uriService.GetSegments(
				uri,
				unescape: true,
				unslash: true,
				unroot: true
			);

			if (paths.Count == 0)
				return Root;

			// add root full path before descendants segments
			paths.Insert(0, Root.FileSystemInfo.FullName);

			string name = paths.Last();
			string physicalPath = Path.Combine(paths.ToArray());
			FileSystemInfo fsi = GetFileSystemInfo(physicalPath, name);

			Fso fso = Create(fsi, name, uri);

			_logger.ChildCreated(fso);

			return fso;
		}

		public Fso Create(Fso parent, FileSystemInfo fsi)
		{
			if (parent == null)
				throw new ArgumentNullException("Child must have a parent", nameof(parent));

			string name = fsi.Name;
			Uri uri = _uriService.GetChild(parent.Uri, name);
			string physicalPath = Path.Combine(parent.FileSystemInfo.FullName, name);

			Fso fso = Create(fsi, name, uri);

			_logger.ChildCreated(fso);

			return fso;
		}

		public List<Fso> CreateChildren(Fso parent)
		{
			if (parent == null)
				throw new ArgumentNullException("Children must have a parent", nameof(parent));

			if (parent.Exists && parent.IsDir)
			{
				return ((DirectoryInfo)parent.FileSystemInfo).EnumerateFileSystemInfos()
					.Where(fsi => !_options.SkipFolders.Contains(fsi.Name, StringComparer.OrdinalIgnoreCase))
					.Select(fsi => Create(parent, fsi))
					.ToList();
			}
			else
			{
				return new List<Fso>(0);
			}
		}

		private Fso Create(FileSystemInfo fsi, string name, Uri uri)
		{
			Core.MediaType mediaType = null;

			if (fsi is FileInfo fi)
			{
				mediaType = _mediaTypeFactoryService.Create(fi.Extension.TrimStart('.'));
			}
			else if (fsi is DirectoryInfo)
			{
				uri = _uriService.GetSlashedUri(uri);
			}

			return new Fso(fsi, name, uri, mediaType, DateTime.Now);
		}

		private FileSystemInfo GetFileSystemInfo(string physicalPath, string name)
		{
			FileAttributes attributes;

			try
			{
				attributes = File.GetAttributes(physicalPath);
			}
			catch (Exception ex)
			{
				_logger.UnknownFileSystemInfo(name, physicalPath, ex);
				return new UnknownFileSystemInfo(name, physicalPath);
			}

			if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
			{
				return new DirectoryInfo(physicalPath);
			}
			else
			{
				return new FileInfo(physicalPath);
			}
		}
	}
}
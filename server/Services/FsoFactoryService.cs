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
	public class FsoFactoryService : IFsoFactoryService
	{
		private readonly ILogger<FsoFactoryService> _logger;
		private readonly AppSettings _options;
		private readonly IFileProvider _fileProvider;
		private readonly IFsoUriService _uriService;
		private readonly IFsoMediaTypeFactoryService _mediaTypeFactoryService;

		public FsoFactoryService(
			ILogger<FsoFactoryService> logger,
			IOptions<AppSettings> optionsAccessor,
			IFileProvider fileProvider,
			IRootFileInfo rootFileInfo,
			IFsoUriService uriService,
			IFsoMediaTypeFactoryService mediaTypeFactoryService)
		{
			_logger = logger;
			_options = optionsAccessor.Value;
			_fileProvider = fileProvider;
			_uriService = uriService;
			_mediaTypeFactoryService = mediaTypeFactoryService;

			// root object
			Root = Create(
				rootFileInfo,
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

			string name = paths.Last();
			string physicalPath = Path.Combine(paths.ToArray());
			IFileInfo fi = _fileProvider.GetFileInfo(physicalPath);
			Fso fso = Create(fi, name, uri);

			_logger.ChildCreated(fso);

			return fso;
		}

		public Fso Create(Fso parent, IFileInfo fi)
		{
			if (parent == null)
				throw new ArgumentNullException("Child must have a parent", nameof(parent));

			string name = fi.Name;
			Uri uri = _uriService.GetChild(parent.Uri, name);
			string physicalPath = Path.Combine(parent.FileInfo.PhysicalPath, name);

			Fso fso = Create(fi, name, uri);

			_logger.ChildCreated(fso);

			return fso;
		}

		public List<Fso> CreateChildren(Fso parent)
		{
			if (parent == null)
				throw new ArgumentNullException("Children must have a parent", nameof(parent));

			if (parent.Exists && parent.IsDir)
			{
				string relativePhysicalPath = GetFileProviderRelativePath(Root.FileInfo.PhysicalPath, parent.FileInfo.PhysicalPath);

				return _fileProvider.GetDirectoryContents(relativePhysicalPath)
					.Where(fi => !_options.SkipFolders.Contains(fi.Name, StringComparer.OrdinalIgnoreCase))
					.Select(fi => Create(parent, fi))
					.ToList();
			}
			else
			{
				return new List<Fso>(0);
			}
		}

		private Fso Create(IFileInfo fi, string name, Uri uri)
		{
			Core.MediaType mediaType = null;

			if (fi.IsDirectory)
			{
				uri = _uriService.GetSlashedUri(uri);
			}
			else
			{
				string extension = Path.GetExtension(fi.Name).TrimStart('.');
				mediaType = _mediaTypeFactoryService.Create(extension);
			}

			return new Fso(fi, name, uri, mediaType, DateTime.Now);
		}

		private string GetFileProviderRelativePath(string parentPath, string childPath)
		{
			string relativePhysicalPath = Path.GetRelativePath(parentPath, childPath);

			if (relativePhysicalPath == ".")
			{
				relativePhysicalPath = string.Empty;
			}

			return relativePhysicalPath;
		}
	}
}
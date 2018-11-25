using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ZeusBrowser.Server.Configuration;
using ZeusBrowser.Server.Core;
using Microsoft.Extensions.Logging;

namespace ZeusBrowser.Server.Services
{
	public class FsoMediaTypeFactoryService
	{
		private readonly ILogger<FsoMediaTypeFactoryService> _logger;
		private readonly Dictionary<string, Tuple<MediaTypeType, string, string>> _cache;

		public FsoMediaTypeFactoryService(
			ILogger<FsoMediaTypeFactoryService> logger,
			IOptions<MediaTypes> mediaTypesOptionsAccessor)
		{
			_logger = logger;
			_cache = BuildCache(mediaTypesOptionsAccessor.Value);
		}

		public Core.MediaType Create(string fileExtension)
		{
			if (_cache.TryGetValue(fileExtension, out var cacheItem))
			{
				(MediaTypeType type, string mimeType, string playerMimeType) = cacheItem.ToValueTuple();

				return new Core.MediaType()
				{
					Type = type,
					MimeType = mimeType,
					PlayerMimeType = playerMimeType
				};
			}
			else
			{
				return new Core.MediaType()
				{
					Type = MediaTypeType.Unknown,
					MimeType = "application/octet-stream"
				};
			}
		}

		private Dictionary<string, Tuple<MediaTypeType, string, string>> BuildCache(MediaTypes options)
		{
			var cache = new Dictionary<string, Tuple<MediaTypeType, string, string>>();

			// so dirty... please don't kill me...
			// i need to found a way to build some lookup object

			var typedMediaTypeSections = new Dictionary<MediaTypeType, Configuration.MediaType>()
			{
				{ MediaTypeType.Video, options.Video },
				{ MediaTypeType.Audio, options.Audio},
				{ MediaTypeType.Subtitles, options.Subtitles },
				{ MediaTypeType.Image, options.Image },
				{ MediaTypeType.Text, options.Text }
			};

			foreach (var typedMediaTypeSection in typedMediaTypeSections)
			{
				var type = typedMediaTypeSection.Key;
				var mediaTypeSection = typedMediaTypeSection.Value;
				var playerMimeTypes = mediaTypeSection.PlayerMimeTypes;

				foreach (var extensionMimeType in mediaTypeSection.MimeTypes)
				{
					var extension = extensionMimeType.Key;
					var mimeType = extensionMimeType.Value;

					if (!cache.ContainsKey(extension))
					{
						cache.Add(
							extension,
							new Tuple<MediaTypeType, string, string>(
								type,
								mimeType,
								playerMimeTypes?.GetValueOrDefault(mimeType, null)
							)
						);
					}
				}
			}

			return cache;
		}
	}
}

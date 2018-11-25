using ZeusBrowser.Server.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeusBrowser.Server.Core;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using ZeusBrowser.Server.Configuration;
using ZeusBrowser.Server.LoggerExtensions;

namespace ZeusBrowser.Server.Services
{
	/// <summary>
	/// <see cref="Fso"/> in-memory cache service
	/// </summary>
	public class FsoCacheService : IFsoCacheService
	{
		private readonly ILogger<FsoCacheService> _logger;
		private readonly AppSettings _options;
		private readonly IMemoryCache _cache;
		private readonly IFsoUriService _uriService;

		/// <summary>
		/// Creates an in-memory cache service instance
		/// </summary>
		/// <param name="cache">ASP.NET Core in-memory caching implementation</param>
		public FsoCacheService(
			ILogger<FsoCacheService> logger,
			IOptions<AppSettings> appOptionsAccessor,
			IMemoryCache cache,
			IFsoUriService uriService)
		{
			_logger = logger;
			_options = appOptionsAccessor.Value;
			_cache = cache;
			_uriService = uriService;
		}

		/// <summary>
		/// True if cache is enabled, else false
		/// </summary>
		public bool IsEnabled => _options.CacheExpiration > TimeSpan.Zero;

		/// <summary>
		/// Try to get an <see cref="Fso"/> by uri
		/// </summary>
		/// <param name="uri">Uri of <see cref="Fso"/> to get</param>
		/// <returns><see cref="Fso"/> or null</returns>
		public Fso this[Uri uri]
		{
			get
			{
				if (TryGet(uri, out Fso fso))
					return fso;

				return null;
			}
			set
			{
				if (value != null)
					Set(value);
				else
					Remove(uri);
			}
		}

		/// <summary>
		/// Try to get related object of parent <see cref="Fso"/> using <paramref name="uri"/> from <paramref name="zone"/> cache zone
		/// </summary>
		/// <param name="fso"><see cref="Fso"/> instance</param>
		/// <returns>List of related <see cref="Fso"/> instances, or null</returns>
		public List<Fso> this[Fso fso, FsoCacheZone zone]
		{
			get
			{
				if (TryGetRelated(fso.Uri, zone, out List<Fso> related))
					return related;

				return null;
			}
			set
			{
				if (value != null)
					SetRelated(fso.Uri, zone, value);
				else
					Remove(fso.Uri, zone);
			}
		}

		/// <summary>
		/// Try to get related object of <paramref name="uri"/> from <paramref name="zone"/> cache zone
		/// </summary>
		/// <param name="uri">Uri</param>
		/// <returns><see cref="Fso"/> or null</returns>
		public List<Fso> this[Uri uri, FsoCacheZone zone]
		{
			get
			{
				if (TryGetRelated(uri, zone, out List<Fso> related))
					return related;

				return null;
			}
			set
			{
				if (value != null)
					SetRelated(uri, zone, value);
				else
					Remove(uri, zone);
			}
		}

		/// <summary>
		/// Try to get an <see cref="Fso"/> by uri
		/// </summary>
		/// <param name="uri">Uri of <see cref="Fso"/> to get</param>
		/// <param name="fso"><see cref="Fso"/> instance retrieved from cache, if successful, or null</param>
		/// <returns>true if retrieved successfully from cache, or false</returns>
		public bool TryGet(Uri uri, out Fso fso)
		{
			_uriService.ValidateUri(uri, nameof(uri));

			var key = GetCacheKey(FsoCacheZone.Singles, uri);

			if (_cache.TryGetValue(key, out fso))
			{
				_logger.CacheHit(key, fso);
				return true;
			}
			else
			{
				_logger.CacheMiss(key);
				return false;
			}
		}

		/// <summary>
		/// Try to get related objects of <paramref name="uri"/> from <paramref name="zone"/> cache zone
		/// </summary>
		/// <param name="uri">Uri of related <see cref="Fso"/></param>
		/// <param name="zone">Cache zone</param>
		/// <param name="related">List of related <see cref="Fso"/> instances retrieved from cache zone, if successful, or null</param>
		/// <returns>true if list retrieved successfully from cache zone, or false</returns>
		public bool TryGetRelated(Uri uri, FsoCacheZone zone, out List<Fso> related)
		{
			_uriService.ValidateUri(uri, nameof(uri));

			if (zone != FsoCacheZone.Children && zone != FsoCacheZone.Descendants)
				throw new ArgumentException($"Cache zone must be {FsoCacheZone.Children} or {FsoCacheZone.Descendants}", nameof(zone));

			var key = GetCacheKey(zone, uri);

			if (_cache.TryGetValue(key, out related))
			{
				_logger.CacheHitRelated(key, related);
				return true;
			}
			else
			{
				_logger.CacheMissRelated(key);
				return false;
			}
		}

		/// <summary>
		/// Store <paramref name="fso"/> in cache
		/// </summary>
		/// <param name="fso"><see cref="Fso"/> instance to store</param>
		public void Set(Fso fso)
		{
			if (fso == null)
				throw new ArgumentNullException(nameof(fso));

			if (!IsEnabled)
				return;

			Set(fso, FsoCacheZone.Singles);
		}

		private void Set(Fso fso, FsoCacheZone zone)
		{
			var key = GetCacheKey(zone, fso.Uri);
			var options = GetCacheOptions(zone);

			_cache.Set(key, fso, options);

			_logger.CacheSet(key);
		}

		/// <summary>
		/// Store related objects of <paramref name="uri"/> in <paramref name="zone"/> cache zone
		/// </summary>
		/// <param name="uri">Uri of related <see cref="Fso"/></param>
		/// <param name="zone">Cache zone</param>
		/// <param name="related">List of related <see cref="Fso"/> instances to store in cache zone</param>
		public void SetRelated(Uri uri, FsoCacheZone zone, List<Fso> related)
		{
			_uriService.ValidateUri(uri, nameof(uri));

			var key = GetCacheKey(zone, uri);
			var options = GetCacheOptions(zone);

			_cache.Set(key, related, options);

			_logger.CacheSetRelated(key, related);
		}

		/// <summary>
		/// Remove <paramref name="fso"/> from cache
		/// </summary>
		/// <param name="fso"><see cref="Fso"/> instance to remove</param>
		public void Remove(Fso fso)
		{
			if (fso == null)
				throw new ArgumentNullException(nameof(fso));

			Remove(fso.Uri);
		}

		/// <summary>
		/// Remove <see cref="Fso"/> instances by uri from all cache zones
		/// </summary>
		/// <param name="uri">Uri of <see cref="Fso"/> instance to remove</param>
		public void Remove(Uri uri)
		{
			_uriService.ValidateUri(uri, nameof(uri));

			foreach (var zone in Enum.GetValues(typeof(FsoCacheZone)).Cast<FsoCacheZone>())
			{
				Remove(uri, zone);
			}
		}

		private void Remove(Uri uri, FsoCacheZone zone)
		{
			var key = GetCacheKey(zone, uri);

			_cache.Remove(key);

			_logger.CacheRemoved(key);
		}

		private Tuple<FsoCacheZone, string> GetCacheKey(FsoCacheZone zone, Uri uri)
		{
			// in-memory cache is surely an hashed-key based dictionary,
			// so there's no need to get a key based on hash code of this zone/uri tuple
			// maybe i should check from myself, but i'm too busy to do some micro-optimizations checks :P
			string path = uri.LocalPath.TrimEnd('/');
			return new Tuple<FsoCacheZone, string>(zone, path);
		}

		private MemoryCacheEntryOptions GetCacheOptions(FsoCacheZone zone)
		{
			var options = new MemoryCacheEntryOptions()
			{
				AbsoluteExpirationRelativeToNow = _options.CacheExpiration,

				// cache item priority:
				// - children are perhaps the most important objects to cache
				// - descendants can be rebuilt from children ones, they're not so important (perhaps they should not be cached)
				// - other (= singles) are kept as normal priority
				Priority = zone == FsoCacheZone.Children
					? CacheItemPriority.High
					: zone == FsoCacheZone.Descendants
						? CacheItemPriority.Low
						: CacheItemPriority.Normal
			};

			var callbackRegistration = new PostEvictionCallbackRegistration()
			{
				EvictionCallback = OnEviction
			};

			options.PostEvictionCallbacks.Add(callbackRegistration);

			return options;
		}

		private void OnEviction(object key, object value, EvictionReason reason, object state)
		{
			_logger.CacheEvicted(key, value, state, reason);
		}
	}
}

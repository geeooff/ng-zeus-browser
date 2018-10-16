using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeusBrowser.Server.Services;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ZeusBrowser.Server.LoggerExtensions
{
    public static class FsoCacheServiceLoggerExtensions
    {
		public static class LoggingEvents
		{
			public const int CacheHit = 4001;
			public const int CacheMiss = 4002;
			public const int CacheHitRelated = 4003;
			public const int CacheMissRelated = 4004;
			public const int CacheSet = 4005;
			public const int CacheSetRelated = 4006;
			public const int CacheRemoved = 4007;
			public const int CacheEvicted = 4008;
			public const int CacheEvictedUnknown = 4009;
		}

		private static readonly Action<ILogger, FsoCacheZone, string, DateTime, TimeSpan, Exception> _cacheHit;
		private static readonly Action<ILogger, FsoCacheZone, string, Exception> _cacheMiss;
		private static readonly Action<ILogger, FsoCacheZone, string, int, Exception> _cacheHitRelated;
		private static readonly Action<ILogger, FsoCacheZone, string, Exception> _cacheMissRelated;
		private static readonly Action<ILogger, FsoCacheZone, string, Exception> _cacheSet;
		private static readonly Action<ILogger, FsoCacheZone, string, int, Exception> _cacheSetRelated;
		private static readonly Action<ILogger, FsoCacheZone, string, Exception> _cacheRemoved;
		private static readonly Action<ILogger, FsoCacheZone, string, EvictionReason, Exception> _cacheEvicted;
		private static readonly Action<ILogger, object, EvictionReason, Exception> _cacheEvictedUnknown;

		static FsoCacheServiceLoggerExtensions()
		{
			_cacheHit = LoggerMessage.Define<FsoCacheZone, string, DateTime, TimeSpan>(
				LogLevel.Trace,
				new EventId(LoggingEvents.CacheHit, nameof(LoggingEvents.CacheHit)),
				"Cache HIT (zone = {Zone}, path = {Path}, created = {Created}, age = {Age})"
			);

			_cacheMiss = LoggerMessage.Define<FsoCacheZone, string>(
				LogLevel.Trace,
				new EventId(LoggingEvents.CacheMiss, nameof(LoggingEvents.CacheMiss)),
				"Cache MISS (zone = {Zone}, path = {Path})"
			);

			_cacheHitRelated = LoggerMessage.Define<FsoCacheZone, string, int>(
				LogLevel.Trace,
				new EventId(LoggingEvents.CacheHitRelated, nameof(LoggingEvents.CacheHitRelated)),
				"Cache HIT (zone = {Zone}, path = {Path}, object count = {Count})"
			);

			_cacheMissRelated = LoggerMessage.Define<FsoCacheZone, string>(
				LogLevel.Trace,
				new EventId(LoggingEvents.CacheMissRelated, nameof(LoggingEvents.CacheMissRelated)),
				"Cache MISS (zone = {Zone}, path = {Path})"
			);

			_cacheSet = LoggerMessage.Define<FsoCacheZone, string>(
				LogLevel.Debug,
				new EventId(LoggingEvents.CacheSet, nameof(LoggingEvents.CacheSet)),
				"Cache SET (zone = {Zone}, path = {Path})"
			);

			_cacheSetRelated = LoggerMessage.Define<FsoCacheZone, string, int>(
				LogLevel.Debug,
				new EventId(LoggingEvents.CacheSetRelated, nameof(LoggingEvents.CacheSetRelated)),
				"Cache SET (zone = {Zone}, path = {Path}, object count = {Count})"
			);

			_cacheRemoved = LoggerMessage.Define<FsoCacheZone, string>(
				LogLevel.Debug,
				new EventId(LoggingEvents.CacheRemoved, nameof(LoggingEvents.CacheRemoved)),
				"Cache REMOVED (zone = {Zone}, path = {Path})"
			);

			_cacheEvicted = LoggerMessage.Define<FsoCacheZone, string, EvictionReason>(
				LogLevel.Debug,
				new EventId(LoggingEvents.CacheEvicted, nameof(LoggingEvents.CacheEvicted)),
				"Cache EVICTED (zone = {Zone}, path = {Path}, reason = {Reason})"
			);

			_cacheEvictedUnknown = LoggerMessage.Define<object, EvictionReason>(
				LogLevel.Debug,
				new EventId(LoggingEvents.CacheEvictedUnknown, nameof(LoggingEvents.CacheEvictedUnknown)),
				"Cache UNKNOWN EVICTED (key = {Key}, reason = {Reason})"
			);
		}

		public static void CacheHit(this ILogger<FsoCacheService> logger, Tuple<FsoCacheZone, string> key, Fso fso)
		{
			_cacheHit(logger, key.Item1, key.Item2, fso.InstanceCreated, fso.InstanceAged, null);
		}

		public static void CacheMiss(this ILogger<FsoCacheService> logger, Tuple<FsoCacheZone, string> key)
		{
			_cacheMiss(logger, key.Item1, key.Item2, null);
		}

		public static void CacheHitRelated(this ILogger<FsoCacheService> logger, Tuple<FsoCacheZone, string> key, List<Fso> related)
		{
			_cacheHitRelated(logger, key.Item1, key.Item2, related.Count, null);
		}

		public static void CacheMissRelated(this ILogger<FsoCacheService> logger, Tuple<FsoCacheZone, string> key)
		{
			_cacheMissRelated(logger, key.Item1, key.Item2, null);
		}

		public static void CacheSet(this ILogger<FsoCacheService> logger, Tuple<FsoCacheZone, string> key)
		{
			_cacheSet(logger, key.Item1, key.Item2, null);
		}

		public static void CacheSetRelated(this ILogger<FsoCacheService> logger, Tuple<FsoCacheZone, string> key, List<Fso> related)
		{
			_cacheSetRelated(logger, key.Item1, key.Item2, related.Count, null);
		}

		public static void CacheRemoved(this ILogger<FsoCacheService> logger, Tuple<FsoCacheZone, string> key)
		{
			_cacheRemoved(logger, key.Item1, key.Item2, null);
		}

		public static void CacheEvicted(this ILogger<FsoCacheService> logger, object key, object value, object state, EvictionReason reason)
		{
			if (key is Tuple<FsoCacheZone, string> typedKey)
			{
				_cacheEvicted(logger, typedKey.Item1, typedKey.Item2, reason, null);
			}
			else
			{
				_cacheEvictedUnknown(logger, key, reason, null);
			}		
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeusBrowser.Server.Services;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;

namespace ZeusBrowser.Server.LoggerExtensions
{
    public static class FsoFactoryServiceLoggerExtensions
	{
		public static class LoggingEvents
		{
			public const int RootCreated = 1001;
			public const int ChildCreated = 1002;
			public const int UnknownFileSystemInfo = 1003;
		}

		private static readonly Action<ILogger, string, string, string, Exception> _rootCreated;
		private static readonly Action<ILogger, string, string, string, Exception> _childCreated;
		private static readonly Action<ILogger, string, string, Exception> _unknownFileSystemInfo;

		static FsoFactoryServiceLoggerExtensions()
		{
			_rootCreated = LoggerMessage.Define<string, string, string>(
				LogLevel.Debug,
				new EventId(LoggingEvents.RootCreated, nameof(LoggingEvents.RootCreated)),
				"Root created (name = {Name}, physical path = {PhysicalPath}, virtual path = {VirtualPath})"
			);

			_childCreated = LoggerMessage.Define<string, string, string>(
				LogLevel.Debug,
				new EventId(LoggingEvents.ChildCreated, nameof(LoggingEvents.ChildCreated)),
				"Child created (name = {Name}, physical path = {PhysicalPath}, virtual path = {VirtualPath})"
			);

			_unknownFileSystemInfo = LoggerMessage.Define<string, string>(
				LogLevel.Error,
				new EventId(LoggingEvents.UnknownFileSystemInfo, nameof(LoggingEvents.UnknownFileSystemInfo)),
				"Can't determine file system info type (name = {Name}, physical path = {PhysicalPath})"
			);
		}

		public static void RootCreated(this ILogger<FsoFactoryService> logger, Fso fso)
		{
			_rootCreated(logger, fso.Name, fso.FileSystemInfo.FullName, fso.Uri.LocalPath, null);
		}

		public static void ChildCreated(this ILogger<FsoFactoryService> logger, Fso fso)
		{
			_childCreated(logger, fso.Name, fso.FileSystemInfo.FullName, fso.Uri.LocalPath, null);
		}

		public static void UnknownFileSystemInfo(this ILogger<FsoFactoryService> logger, string name, string physicalPath, Exception ex)
		{
			_unknownFileSystemInfo(logger, name, physicalPath, ex);
		}
	}
}

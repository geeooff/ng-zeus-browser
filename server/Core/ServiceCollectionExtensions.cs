using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeusBrowser.Server.Services;
using ZeusBrowser.Server.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Options;
using System.IO;

namespace ZeusBrowser.Server.Core
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddZeusBrowser(this IServiceCollection services, IConfiguration configuration)
		{
			// app configuration
			services.Configure<AppSettings>(configuration);
			services.Configure<MediaTypes>(configuration.GetSection(nameof(MediaTypes)));

			// app physical file provider, based on configured root path
			services.AddSingleton<IFileProvider>(serviceProvider =>
			{
				var appSettingsAccessor = serviceProvider.GetService<IOptions<AppSettings>>();
				return new PhysicalFileProvider(
					appSettingsAccessor.Value.PhysicalPath,
					ExclusionFilters.Sensitive
				);
			});

			// root file info, required by some services who need to access unavailable data of the file provider
			services.AddSingleton<IRootFileInfo>(serviceProvider =>
			{
				var appSettingsAccessor = serviceProvider.GetService<IOptions<AppSettings>>();
				return new RootFileInfo(
					new PhysicalDirectoryInfo(
						new DirectoryInfo(appSettingsAccessor.Value.PhysicalPath)
					)
				);
			});

			// media type builder service
			// role: resolve media types from file system objects
			services.AddSingleton<IFsoMediaTypeFactoryService, FsoMediaTypeFactoryService>();

			// file system object uri service
			// role: helper class that handles file system uris
			services.AddSingleton<IFsoUriService, FsoUriService>();

			// file system object builder service
			// role: creates instances of file system objects
			services.AddSingleton<IFsoFactoryService, FsoFactoryService>();

			// file system object cache service
			// role: retrieve and store file system objects in-memory
			services.AddMemoryCache();
			services.AddSingleton<IFsoCacheService, FsoCacheService>();

			// playlist service
			// role: generates playlist files from fsos
			services.AddSingleton<IFsoPlaylistService, FsoPlaylistService>();

			// mediainfo service
			// role: get media informations from mediainfo executable
			services.AddSingleton<IFsoMediaInfoService, FsoMediaInfoService>();

			// file system object repository
			// role: interrogates cache or factory to retrieve file system objects corresponding to http context
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IFsoRepositoryService, FsoRepositoryService>();

			return services;
		}
	}
}

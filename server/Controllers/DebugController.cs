#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ZeusBrowser.Server.Configuration;
using ZeusBrowser.Server.Services;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;
using Microsoft.Extensions.Logging;

namespace ZeusBrowser.Server.Controllers
{
	[Route("api/debug")]
	[ApiController]
	public class DebugController : Controller
	{
		private readonly ILogger<DebugController> _logger;
		private readonly AppSettings _options;
		private readonly MediaTypes _mediaTypesOptions;
		private readonly IFsoUriService _uriService;
		private readonly IFsoRepositoryService _repositoryService;
		private readonly IFsoPlaylistService _playlistService;

		public DebugController(
			ILogger<DebugController> logger,
			IOptions<AppSettings> optionsAccessor,
			IOptions<MediaTypes> mediaTypesOptionsAccessor,
			IFsoUriService uriService,
			IFsoRepositoryService repositoryService,
			IFsoPlaylistService playlistService)
		{
			_logger = logger;
			_options = optionsAccessor.Value;
			_mediaTypesOptions = mediaTypesOptionsAccessor.Value;
			_uriService = uriService;
			_repositoryService = repositoryService;
			_playlistService = playlistService;
		}

		[HttpGet()]
		public IActionResult Config()
		{
			return Ok(
				new
				{
					app = _options,
					mediaTypes = _mediaTypesOptions
				}
			);
		}

		[HttpGet("{bda}/{*path}")]
		public IActionResult Builder(string path, BuilderDebugActions bda = BuilderDebugActions.GetChain, MediaTypeType? mt = null, GroupBy gb = Defaults.GroupBy, OrderBy ob = Defaults.OrderBy)
		{
			Uri uri = _uriService.Get(path);
			Fso fso = _repositoryService.Get(uri);
			
			if (!fso.Exists)
				return NotFound(fso);

			switch (bda)
			{
				case BuilderDebugActions.GetChain:
					var chain = _repositoryService.GetAncestors(uri).Append(fso);
					return Ok(chain);

				case BuilderDebugActions.GetChildren:
					var children = _repositoryService.GetChildren(uri, gb, ob);
					return Ok(new
					{
						fso,
						children
					});

				case BuilderDebugActions.GetSiblings:
					var siblings = _repositoryService.GetSiblings(uri, gb, ob, out Fso previous, out Fso next, andSelf: false);
					return Ok(new
					{
						fso,
						siblings,
						previous,
						next
					});

				//case BuilderDebugActions.GetAllFiles:
				//	var allFiles = fso.GetAllFiles(mt, gb, ob);
				//	return Ok(new
				//	{
				//		chain,
				//		allFiles
				//	});

				case BuilderDebugActions.GetSecureLink:
					return Ok(new
					{
						fso,
						secureLink = _uriService.GetSecureLink(uri, HttpContext)
					});

				//case BuilderDebugActions.GetSecureLinks:
				//	var descendants = _repositoryService.GetDescendants(uri, gb, ob, mt);
				//	return File(
				//		_playlistService.GetUrls(descendants, HttpContext),
				//		"text/plain; charset=utf-8",
				//		"urls.txt"
				//	);

				//case BuilderDebugActions.GetM3U:
				//	var descendants = _repositoryService.GetDescendants(uri, gb, ob, mt);
				//	return File(
				//		fso.GetM3U(HttpContext, mt, gb, ob),
				//		"application/x-mpegurl; charset=utf-8",
				//		"playlist.m3u8"
				//	);

				//case BuilderDebugActions.GetASX:
				//	var descendants = _repositoryService.GetDescendants(uri, gb, ob, mt);
				//	return File(
				//		fso.GetASX(HttpContext, mt, gb, ob),
				//		"application/x-mpegurl; charset=utf-8",
				//		"playlist.asx"
				//	);

				default:
					return BadRequest();
			}
		}

		public enum BuilderDebugActions
		{
			GetChain,
			GetChildren,
			GetSiblings,
			GetAllFiles,
			GetSecureLink,
			GetSecureLinks,
			GetM3U,
			GetASX
		}
	}
}
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;
using ZeusBrowser.Server.Services;

namespace ZeusBrowser.Server.Controllers
{
	[Route("api/playlist")]
	[ApiController]
	public class PlaylistController : ControllerBase
    {
		private readonly ILogger<PlaylistController> _logger;
		private readonly FsoUriService _uriService;
		private readonly FsoRepositoryService _repositoryService;
		private readonly FsoPlaylistService _playlistService;

		public PlaylistController(
			ILogger<PlaylistController> logger,
			FsoUriService uriService,
			FsoRepositoryService repositoryService,
			FsoPlaylistService playlistService)
		{
			_logger = logger;
			_uriService = uriService;
			_repositoryService = repositoryService;
			_playlistService = playlistService;
		}

		[HttpGet("m3u/{*path}")]
		[Produces("application/x-mpegurl")]
		public FileResult GetM3U(
			string path = Defaults.Path,
			GroupBy groupBy = Defaults.GroupBy,
			OrderBy orderBy = Defaults.OrderBy,
			MediaTypeType? mediaType = null)
		{
			var uri = _uriService.Get(path);
			var descendants = _repositoryService.GetDescendants(uri, groupBy, orderBy, mediaType);

			return File(
				_playlistService.GetM3U(descendants, HttpContext),
				"application/x-mpegurl; charset=utf-8",
				"playlist.m3u8"
			);
		}

		[HttpGet("asx/{*path}")]
		[Produces("video/x-ms-asf")]
		public FileResult GetASX(
			string path = Defaults.Path,
			GroupBy groupBy = Defaults.GroupBy,
			OrderBy orderBy = Defaults.OrderBy,
			MediaTypeType? mediaType = null)
		{
			var uri = _uriService.Get(path);
			var fso = _repositoryService.Get(uri);
			var descendants = _repositoryService.GetDescendants(fso.Uri, groupBy, orderBy, mediaType);

			return File(
				_playlistService.GetASX(fso.Name, descendants, HttpContext),
				"video/x-ms-asf; charset=utf-8",
				"playlist.asx"
			);
		}

		[HttpGet("urls/{*path}")]
		[Produces("text/plain")]
		public FileResult GetUrls(
			string path = Defaults.Path,
			GroupBy groupBy = Defaults.GroupBy,
			OrderBy orderBy = Defaults.OrderBy,
			MediaTypeType? mediaType = null)
		{
			var uri = _uriService.Get(path);
			var descendants = _repositoryService.GetDescendants(uri, groupBy, orderBy, mediaType);

			return File(
				_playlistService.GetUrls(descendants, HttpContext),
				"text/plain; charset=utf-8",
				"urls.txt"
			);
		}
	}
}
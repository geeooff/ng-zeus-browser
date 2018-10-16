using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Services;

namespace ZeusBrowser.Server.Controllers
{
	[Route("api/mediainfo")]
	[ApiController]
    public class MediaInfoController : Controller
    {
		private readonly ILogger<MediaInfoController> _logger;
		private readonly FsoUriService _uriService;
		private readonly FsoRepositoryService _repositoryService;
		private readonly FsoMediaInfoService _mediaInfoService;

		public MediaInfoController(
			ILogger<MediaInfoController> logger,
			FsoUriService uriService,
			FsoRepositoryService repositoryService,
			FsoMediaInfoService mediaInfoService)
		{
			_logger = logger;
			_uriService = uriService;
			_repositoryService = repositoryService;
			_mediaInfoService = mediaInfoService;
		}

		[HttpGet("text/{*path}")]
		[Produces("text/plain")]
		public ContentResult GetText(string path = Defaults.Path)
		{
			var uri = _uriService.Get(path);
			var fso = _repositoryService.Get(uri);
			string output = _mediaInfoService.Get(fso, MediaInfoOutput.Text);

			return Content(output, "text/plain");
		}

		[HttpGet("xml/{*path}")]
		[Produces("application/xml")]
		public ContentResult GetXML(string path = Defaults.Path)
		{
			var uri = _uriService.Get(path);
			var fso = _repositoryService.Get(uri);
			string output = _mediaInfoService.Get(fso, MediaInfoOutput.Xml);

			return Content(output, "application/xml");
		}

		[HttpGet("json/{*path}")]
		[Produces("application/json")]
		public ContentResult GetJson(string path = Defaults.Path)
		{
			var uri = _uriService.Get(path);
			var fso = _repositoryService.Get(uri);
			string output = _mediaInfoService.Get(fso, MediaInfoOutput.Json);

			return Content(output, "application/json");
		}

		[HttpGet("html/{*path}")]
		[Produces("text/html")]
		public ContentResult GetHtml(string path = Defaults.Path)
		{
			var uri = _uriService.Get(path);
			var fso = _repositoryService.Get(uri);
			string output = _mediaInfoService.Get(fso, MediaInfoOutput.Html);

			return Content(output, "text/html");
		}
	}
}

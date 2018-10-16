using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Services;
using ZeusBrowser.Server.Models;
using ZeusBrowser.Server.Models.Browse;

namespace ZeusBrowser.Server.Controllers
{
    [Route("api/browse")]
    [ApiController]
    public class BrowseController : ControllerBase
    {
		private readonly ILogger<BrowseController> _logger;
		private readonly FsoUriService _uriService;
		private readonly FsoRepositoryService _repositoryService;

		public BrowseController(
			ILogger<BrowseController> logger,
			FsoUriService uriService,
			FsoRepositoryService repositoryService)
		{
			_logger = logger;
			_uriService = uriService;
			_repositoryService = repositoryService;
		}

		[HttpGet("single/{*path}")]
		public ActionResult<SingleResult> GetSingle(
			string path = Defaults.Path)
		{
			var uri = _uriService.Get(path);
			var fso = _repositoryService.Get(uri);
			var ancestors = _repositoryService.GetAncestors(uri);

			return new SingleResult
			{
				Fso = fso,
				Ancestors = ancestors
			};
		}

		[HttpGet("siblings/{*path}")]
		public ActionResult<SiblingsResult> GetSiblings(
			string path = Defaults.Path,
			GroupBy groupBy = Defaults.GroupBy,
			OrderBy orderBy = Defaults.OrderBy)
		{
			var uri = _uriService.Get(path);
			var siblings = _repositoryService.GetSiblings(uri, groupBy, orderBy, out Fso previous, out Fso next, andSelf: true);

			return new SiblingsResult
			{
				Path = uri.AbsolutePath,
				GroupBy = groupBy,
				OrderBy = orderBy,
				Siblings = siblings
					.Select(fso => new PathResult
					{
						Path = fso.Path
					})
					.ToList(),
				Previous = previous,
				Next = next
			};
		}

		[HttpGet("children/{*path}")]
		public ActionResult<ChildrenResult> GetChildren(
			string path = Defaults.Path,
			GroupBy groupBy = Defaults.GroupBy,
			OrderBy orderBy = Defaults.OrderBy)
		{
			var uri = _uriService.Get(path);
			var children = _repositoryService.GetChildren(uri, groupBy, orderBy);

			return new ChildrenResult
			{
				Path = uri.AbsolutePath,
				GroupBy = groupBy,
				OrderBy = orderBy,
				Children = children
			};
		}

		[HttpGet("descendants/{*path}")]
		public ActionResult<DescendantsResult> GetDescendants(
			string path = Defaults.Path,
			GroupBy groupBy = Defaults.GroupBy,
			OrderBy orderBy = Defaults.OrderBy,
			MediaTypeType? mediaType = null)
		{
			var uri = _uriService.Get(path);
			var descendants = _repositoryService.GetDescendants(uri, groupBy, orderBy, mediaType);

			return new DescendantsResult
			{
				Path = uri.AbsolutePath,
				GroupBy = groupBy,
				OrderBy = orderBy,
				MediaType = mediaType,
				Descendants = descendants
			};
		}
	}
}
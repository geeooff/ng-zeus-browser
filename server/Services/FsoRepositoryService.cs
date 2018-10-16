using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;
using ZeusBrowser.Server.Configuration;
using Microsoft.AspNetCore.Http.Headers;

namespace ZeusBrowser.Server.Services
{
    public class FsoRepositoryService
    {
		private readonly ILogger<FsoRepositoryService> _logger;
		private readonly AppSettings _options;
		private readonly FsoUriService _uriService;
		private readonly FsoCacheService _cacheService;
		private readonly FsoFactoryService _factoryService;
		private readonly HttpContext _httpContext;
		private readonly RequestHeaders _requestHeaders;

		public FsoRepositoryService(
			ILogger<FsoRepositoryService> logger,
			IOptions<AppSettings> optionsAccessor,
			FsoUriService uriService,
			FsoCacheService cacheService,
			FsoFactoryService factoryService,
			IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			_options = optionsAccessor.Value;
			_uriService = uriService;
			_cacheService = cacheService;
			_factoryService = factoryService;
			_httpContext = httpContextAccessor.HttpContext;
			_requestHeaders = _httpContext.Request.GetTypedHeaders();
		}

		public Fso Root => _factoryService.Root;

		public bool NoCacheRequested => (_requestHeaders.CacheControl?.NoCache ?? false);

		public Fso GetCurrent()
		{
			throw new NotImplementedException();
		}

		public Fso Get(Uri uri)
		{
			_uriService.ValidateUri(uri, nameof(uri));

			if (_uriService.IsRootUri(uri))
				return Root;

			if (NoCacheRequested
			 ||	!_cacheService.IsEnabled
			 || !_cacheService.TryGet(uri, out Fso fso))
			{
				fso = _factoryService.Create(uri);

				if (_cacheService.IsEnabled)
				{
					_cacheService.Set(fso);
				}
			}

			return fso;
		}

		public Fso GetParent(Uri uri)
		{
			_uriService.ValidateUri(uri, nameof(uri));

			if (_uriService.IsRootUri(uri))
				return null;

			Uri parentUri = _uriService.GetParent(uri);

			return Get(parentUri);
		}

		public List<Fso> GetAncestors(Uri uri)
		{
			_uriService.ValidateUri(uri, nameof(uri));

			if (_uriService.IsRootUri(uri))
				return new List<Fso>(0);

			List<Uri> ancestorsUris = _uriService.GetAncestors(uri);

			return ancestorsUris
				.Select(ancestorUri => Get(ancestorUri))
				.ToList();
		}

		public List<Fso> GetChildren(Uri uri, GroupBy groupBy, OrderBy orderBy)
		{
			_uriService.ValidateUri(uri, nameof(uri));

			if (NoCacheRequested
			 || !_cacheService.IsEnabled
			 || !_cacheService.TryGetRelated(uri, FsoCacheZone.Children, out List<Fso> children))
			{
				Fso fso = Get(uri);

				children = _factoryService.CreateChildren(fso);

				if (_cacheService.IsEnabled)
				{
					foreach (Fso child in children)
					{
						_cacheService.Set(child);
					}

					_cacheService.SetRelated(fso.Uri, FsoCacheZone.Children, children);
				}
			}

			return children.OrderObjectsBy(groupBy, orderBy).ToList();
		}

		public List<Fso> GetSiblings(Uri uri, GroupBy groupBy, OrderBy orderBy, out Fso previous, out Fso next, bool andSelf = false)
		{
			_uriService.ValidateUri(uri, nameof(uri));

			previous = null;
			next = null;

			if (_uriService.IsRootUri(uri))
			{
				return new List<Fso>(0);
			}

			Uri parentUri = _uriService.GetParent(uri);
			var siblings = GetChildren(parentUri, groupBy, orderBy);

			int selfIndex = siblings.FindIndex(fso => _uriService.Equals(uri, fso.Uri));

			if (selfIndex != -1)
			{
				int maxIndex = siblings.Count - 1;

				if (selfIndex > 0)
				{
					previous = siblings.ElementAt(selfIndex - 1);
				}
				if (selfIndex < maxIndex)
				{
					next = siblings.ElementAt(selfIndex + 1);
				}

				if (!andSelf && selfIndex > 0)
				{
					siblings.RemoveAt(selfIndex);
				}
			}

			return siblings;
		}

		public List<Fso> GetDescendants(Uri uri, GroupBy groupBy, OrderBy orderBy, MediaTypeType? filterMediaType)
		{
			_uriService.ValidateUri(uri, nameof(uri));

			if (NoCacheRequested
			 || !_cacheService.IsEnabled
			 || !_cacheService.TryGetRelated(uri, FsoCacheZone.Descendants, out List<Fso> descendants))
			{
				Fso fso = Get(uri);

				if (fso.Exists)
				{
					descendants = GetDescendantsRecursive(fso, groupBy, orderBy).ToList();
				}
				else
				{
					descendants = new List<Fso>(0);
				}

				_cacheService.SetRelated(fso.Uri, FsoCacheZone.Descendants, descendants);
			}

			if (filterMediaType.HasValue)
			{
				descendants = descendants
					.Where(fso => fso.IsFile && fso.MediaType.Type == filterMediaType.Value)
					.ToList();
			}

			return descendants;
		}

		// Need serious refactoring...
		private IEnumerable<Fso> GetDescendantsRecursive(Fso fso, GroupBy groupBy, OrderBy orderBy)
		{
			if (fso.IsDir)
			{
				foreach (Fso child in GetChildren(fso.Uri, groupBy, orderBy))
				{
					foreach (Fso grandChild in GetDescendantsRecursive(child, groupBy, orderBy))
					{
						yield return grandChild;
					}
				}
			}
			else if (fso.IsFile)
			{
				yield return fso;
			}
		}
	}
}

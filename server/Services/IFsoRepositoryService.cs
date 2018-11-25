using System;
using System.Collections.Generic;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;

namespace ZeusBrowser.Server.Services
{
	public interface IFsoRepositoryService
	{
		bool NoCacheRequested { get; }
		Fso Root { get; }

		Fso Get(Uri uri);
		List<Fso> GetAncestors(Uri uri);
		List<Fso> GetChildren(Uri uri, GroupBy groupBy, OrderBy orderBy);
		Fso GetCurrent();
		List<Fso> GetDescendants(Uri uri, GroupBy groupBy, OrderBy orderBy, MediaTypeType? filterMediaType);
		Fso GetParent(Uri uri);
		List<Fso> GetSiblings(Uri uri, GroupBy groupBy, OrderBy orderBy, out Fso previous, out Fso next, bool andSelf = false);
	}
}
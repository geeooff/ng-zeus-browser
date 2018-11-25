using System;
using System.Collections.Generic;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;

namespace ZeusBrowser.Server.Services
{
	public interface IFsoCacheService
	{
		Fso this[Uri uri] { get; set; }
		List<Fso> this[Fso fso, FsoCacheZone zone] { get; set; }
		List<Fso> this[Uri uri, FsoCacheZone zone] { get; set; }

		bool IsEnabled { get; }

		void Remove(Fso fso);
		void Remove(Uri uri);
		void Set(Fso fso);
		void SetRelated(Uri uri, FsoCacheZone zone, List<Fso> related);
		bool TryGet(Uri uri, out Fso fso);
		bool TryGetRelated(Uri uri, FsoCacheZone zone, out List<Fso> related);
	}
}
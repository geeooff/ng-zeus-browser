using System;
using System.Collections.Generic;
using System.IO;
using ZeusBrowser.Server.Models;

namespace ZeusBrowser.Server.Services
{
	public interface IFsoFactoryService
	{
		Fso Root { get; }

		Fso Create(Fso parent, FileSystemInfo fsi);
		Fso Create(Uri uri);
		List<Fso> CreateChildren(Fso parent);
	}
}
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using ZeusBrowser.Server.Models;

namespace ZeusBrowser.Server.Services
{
	public interface IFsoPlaylistService
	{
		Stream GetASX(string title, IEnumerable<Fso> fsos, HttpContext httpContext);
		Stream GetM3U(IEnumerable<Fso> fsos, HttpContext httpContext);
		Stream GetUrls(IEnumerable<Fso> fsos, HttpContext httpContext);
	}
}
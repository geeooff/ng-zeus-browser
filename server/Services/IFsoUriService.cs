using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ZeusBrowser.Server.Services
{
	public interface IFsoUriService
	{
		bool AreExternalUrisEnabled { get; }
		bool AreSecureLinkUrisEnabled { get; }

		bool Equals(Uri uri1, Uri uri2);
		Uri Get(string path);
		List<Uri> GetAncestors(Uri uri);
		Uri GetChild(Uri parentUri, string childName);
		Uri GetDownload(Uri uri, HttpContext httpContext);
		Uri GetExternal(Uri uri);
		Uri GetParent(Uri uri);
		Uri GetSecureLink(Uri uri, HttpContext httpContext);
		List<string> GetSegments(Uri uri, bool unescape = false, bool unslash = false, bool unroot = false);
		Uri GetSlashedUri(Uri uri);
		bool IsRootUri(Uri uri);
		void ValidateUri(Uri uri, string paramName);
	}
}
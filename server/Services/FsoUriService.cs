using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ZeusBrowser.Server.Configuration;
using ZeusBrowser.Server.Core;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace ZeusBrowser.Server.Services
{
	public class FsoUriService : IFsoUriService
	{
		public const string Scheme = Defaults.InternalUriScheme;
		public const string Host = Defaults.InternetUriHost;

		private readonly ILogger<FsoUriService> _logger;
		private readonly AppSettings _options;
		private readonly Uri _rootInternalUri;

		public FsoUriService(
			ILogger<FsoUriService> logger,
			IOptions<AppSettings> optionsAccessor)
		{
			_logger = logger;
			_options = optionsAccessor.Value;

			UriBuilder rootInternalUriBuilder = new UriBuilder(Scheme, Host);

			_rootInternalUri = rootInternalUriBuilder.Uri;
		}

		public bool AreExternalUrisEnabled
		{
			get
			{
				return (_options.ExternalPath != null);
			}
		}

		public bool AreSecureLinkUrisEnabled
		{
			get
			{
				return (_options.SecureLinks.Path != null);
			}
		}

		public Uri Get(string path)
		{
			UriBuilder uriBuilder = new UriBuilder(_rootInternalUri)
			{
				Path = !string.IsNullOrWhiteSpace(path)
					? Uri.EscapeUriString(path)
					: "/"
			};
			return uriBuilder.Uri;
		}

		public List<string> GetSegments(Uri uri, bool unescape = false, bool unslash = false, bool unroot = false)
		{
			ValidateUri(uri, nameof(uri));

			return uri
				.Segments
				.Select(seg => {
					if (unslash && seg != "/")
						seg = seg.TrimEnd('/');
					if (unescape)
						seg = Uri.UnescapeDataString(seg);					
					return seg;
				})
				.Where(seg => !unroot || (seg != string.Empty && seg != "/"))
				.ToList();
		}
		
		public Uri GetParent(Uri uri)
		{
			ValidateUri(uri, nameof(uri));

			if (IsRootUri(uri))
				return null;

			if (uri.AbsolutePath.EndsWith('/'))
				return new Uri(uri, "..");
			else
				return new Uri(uri, ".");
		}

		public bool IsRootUri(Uri uri)
		{
			return uri.AbsolutePath == "/";
		}

		public Uri GetChild(Uri parentUri, string childName)
		{
			ValidateUri(parentUri, nameof(parentUri));

			childName = Uri.EscapeDataString(childName);
			parentUri = GetSlashedUri(parentUri);

			return new Uri(parentUri, childName);
		}

		public Uri GetSlashedUri(Uri uri)
		{
			if (!uri.AbsolutePath.EndsWith('/'))
			{
				string path = Uri.UnescapeDataString(uri.AbsolutePath) + '/';
				return Get(path);
			}
			return uri;
		}

		public List<Uri> GetAncestors(Uri uri)
		{
			ValidateUri(uri, nameof(uri));

			List<Uri> uris = new List<Uri>();

			while ((uri = GetParent(uri)) != null)
			{
				uris.Add(uri);
			}

			return uris;
		}

		public Uri GetExternal(Uri uri)
		{
			if (!AreExternalUrisEnabled)
				throw new InvalidOperationException("External URIs are disabled");

			ValidateUri(uri, nameof(uri));

			Uri relativeUri = GetRelativeInternalUri(uri);
			return new Uri(_options.ExternalPath, relativeUri);
		}

		public Uri GetSecureLink(Uri uri, HttpContext httpContext)
		{
			if (!AreSecureLinkUrisEnabled)
				throw new InvalidOperationException("Secure link URIs are disabled");

			ValidateUri(uri, nameof(uri));

			Uri relativeUri = GetRelativeInternalUri(uri);
			Uri absoluteUri = new Uri(_options.SecureLinks.Path, relativeUri);

			// Specifications: https://nginx.org/en/docs/http/ngx_http_secure_link_module.html

			DateTimeOffset expires = DateTimeOffset.Now.Add(_options.SecureLinks.Expiration);
			string sUri = absoluteUri.GetComponents(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.SafeUnescaped);
			string sExpires = expires.ToUnixTimeSeconds().ToString();
			string sRemoteAddr = httpContext.Connection.RemoteIpAddress.ToString();

			string value = _options.SecureLinks.Format
				.Replace("$uri", sUri)
				.Replace("$secure_link_expires", sExpires)
				.Replace("$remote_addr", sRemoteAddr);

			byte[] bytesIn = Encoding.ASCII.GetBytes(value);
			byte[] bytesOut;

			using (var hashAlg = MD5.Create())
			{
				bytesOut = hashAlg.ComputeHash(bytesIn);
				hashAlg.Clear();
			}

			//// https://tools.ietf.org/html/rfc4648#section-5
			//string hashValue = Convert.ToBase64String(bytesOut)
			//	.Replace('+', '-')
			//	.Replace('/', '_')
			//	.Replace("=", "");
			string hashValue = WebEncoders.Base64UrlEncode(bytesOut);

			UriBuilder uriBuilder = new UriBuilder(absoluteUri);

			QueryBuilder queryBuilder = new QueryBuilder()
			{
				{ _options.SecureLinks.Args.MD5, hashValue },
				{ _options.SecureLinks.Args.Expires, sExpires }
			};

			uriBuilder.Query = queryBuilder.ToString();

			return uriBuilder.Uri;
		}

		public Uri GetDownload(Uri uri, HttpContext httpContext)
		{
			ValidateUri(uri, nameof(uri));

			if (AreSecureLinkUrisEnabled)
			{
				return GetSecureLink(uri, httpContext);
			}
			else if (AreExternalUrisEnabled)
			{
				return GetExternal(uri);
			}
			else
			{
				throw new InvalidOperationException("External and Secure Link URIs are both disabled. Can't generate playlist item URI.");
			}
		}

		public Uri GetRelativeInternalUri(Uri uri)
		{
			return _rootInternalUri.MakeRelativeUri(uri);
		}

		public void ValidateUri(Uri uri, string paramName)
		{
			if (uri == null)
				throw new ArgumentNullException(paramName, "Specified URI is null");

			if (!uri.IsAbsoluteUri || uri.Scheme != Scheme || uri.Host != Host)
				throw new ArgumentException(paramName, $"Specified URI \"{uri}\" is not a valid uri");
		}

		public bool Equals(Uri uri1, Uri uri2)
		{
			ValidateUri(uri1, nameof(uri1));
			ValidateUri(uri2, nameof(uri2));

			return uri1.LocalPath.TrimEnd('/') == uri2.LocalPath.TrimEnd('/');
		}
	}
}

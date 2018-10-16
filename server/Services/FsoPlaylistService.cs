using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace ZeusBrowser.Server.Services
{
    public class FsoPlaylistService
    {
		private readonly ILogger<FsoPlaylistService> _logger;
		private readonly FsoUriService _uriService;

		public FsoPlaylistService(
			ILogger<FsoPlaylistService> logger,
			FsoUriService uriService)
		{
			_logger = logger;
			_uriService = uriService;
		}

		public Stream GetM3U(IEnumerable<Fso> fsos, HttpContext httpContext)
		{
			MemoryStream stream = new MemoryStream();

			using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, Defaults.StreamWriterBufferSize, true))
			{
				writer.WriteLine("#EXTM3U");
				writer.WriteLine();

				foreach (Fso fso in fsos)
				{
					Uri secureLink = _uriService.GetDownload(fso.Uri, httpContext);

					writer.Write("#EXTINF:-1;");
					writer.Write(fso.Name);
					writer.WriteLine();
					writer.WriteLine(secureLink.AbsoluteUri);
					writer.WriteLine();
				}

				writer.Close();
			}

			stream.Position = 0;

			return stream;
		}

		public Stream GetASX(string title, IEnumerable<Fso> fsos, HttpContext httpContext)
		{
			MemoryStream stream = new MemoryStream();

			XmlWriterSettings settings = new XmlWriterSettings()
			{
				CloseOutput = false,
				Encoding = Encoding.UTF8,
				Indent = true,
				IndentChars = "\t"
			};

			using (XmlWriter writer = XmlWriter.Create(stream, settings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("asx");
				writer.WriteAttributeString("version", "3.0");
				writer.WriteElementString("title", title);

				foreach (Fso fso in fsos)
				{
					Uri downloadUri = _uriService.GetDownload(fso.Uri, httpContext);

					writer.WriteStartElement("entry");
					writer.WriteElementString("title", fso.Name);
					writer.WriteStartElement("ref");
					writer.WriteAttributeString("href", downloadUri.AbsoluteUri);
					writer.WriteEndElement();
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.Flush();
				writer.Close();
			}

			stream.Position = 0;

			return stream;
		}

		public Stream GetUrls(IEnumerable<Fso> fsos, HttpContext httpContext)
		{
			MemoryStream stream = new MemoryStream();

			using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, Defaults.StreamWriterBufferSize, true))
			{
				foreach (Fso fso in fsos)
				{
					Uri downloadUri = _uriService.GetDownload(fso.Uri, httpContext);

					writer.WriteLine(downloadUri.AbsoluteUri);
				}

				writer.Flush();
				writer.Close();
			}

			stream.Position = 0;

			return stream;
		}		
	}
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeusBrowser.Server.Configuration;
using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;

namespace ZeusBrowser.Server.Services
{
	public class FsoMediaInfoService : IFsoMediaInfoService
	{
		private readonly ILogger<FsoMediaInfoService> _logger;
		private readonly AppSettings _options;

		public FsoMediaInfoService(
			ILogger<FsoMediaInfoService> logger,
			IOptions<AppSettings> optionsAccessor)
		{
			_logger = logger;
			_options = optionsAccessor.Value;
		}

		public string Get(Fso fso, MediaInfoOutput output)
		{
			string mediaInfoArgs = string.Format(
				"\"{0}\" --Output={1}",
				fso.FileSystemInfo.FullName,
				output.ToString().ToUpper()
			);

			ProcessStartInfo startInfo = new ProcessStartInfo(_options.MediaInfoBin, mediaInfoArgs)
			{
				UseShellExecute = false,
				RedirectStandardOutput = true,
				StandardOutputEncoding = Encoding.UTF8
			};

			try
			{
				using (Process ps = Process.Start(startInfo))
				{
					string stdOutput = ps.StandardOutput.ReadToEnd();

					int timeout = Defaults.MediaInfoProcessTimeout;

					if (!ps.WaitForExit(timeout))
					{
						// TODO logging

						try
						{
							ps.Kill();
						}
						catch (Exception ex)
						{
							throw new MediaInfoTimeoutException($"MediaInfo process was unresponsive after waiting {timeout} ms and was unable to be killed", ex);
						}

						throw new MediaInfoTimeoutException($"MediaInfo process was unresponsive after waiting {timeout} ms and was killed");
					}

					if (ps.ExitCode != 0)
					{
						throw new MediaInfoExitCodeException($"MediaInfo process exited with code {ps.ExitCode}");
					}

					return stdOutput;
				}
			}
			catch (Exception ex)
			{
				if (!(ex is MediaInfoException))
				{
					throw new MediaInfoException($"Unhandled exception while processing MediaInfo output", ex);
				}
				throw;
			}
		}
	}
}

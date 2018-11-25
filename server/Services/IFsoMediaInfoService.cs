using ZeusBrowser.Server.Core;
using ZeusBrowser.Server.Models;

namespace ZeusBrowser.Server.Services
{
	public interface IFsoMediaInfoService
	{
		string Get(Fso fso, MediaInfoOutput output);
	}
}
using ZeusBrowser.Server.Core;

namespace ZeusBrowser.Server.Services
{
	public interface IFsoMediaTypeFactoryService
	{
		MediaType Create(string fileExtension);
	}
}
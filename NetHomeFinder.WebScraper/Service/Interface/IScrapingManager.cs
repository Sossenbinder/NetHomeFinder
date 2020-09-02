using System.Threading;
using System.Threading.Tasks;

namespace NetHomeFinder.WebScraper.Service.Interface
{
	public interface IScrapingManager
	{
		Task StartContinuousScraping(CancellationToken? cancellationToken = null);
	}
}
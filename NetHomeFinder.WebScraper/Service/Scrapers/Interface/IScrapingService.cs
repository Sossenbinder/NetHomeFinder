using System.Collections.Generic;
using System.Threading.Tasks;
using NetHomeFinder.Common.Definitions.Models;

namespace NetHomeFinder.WebScraper.Service.Scrapers.Interface
{
	public interface IScrapingService
	{
		Task<IEnumerable<Estate>> GetData();
	}
}
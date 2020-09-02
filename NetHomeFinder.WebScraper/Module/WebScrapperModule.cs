using Autofac;
using NetHomeFinder.WebScraper.Service;
using NetHomeFinder.WebScraper.Service.Interface;
using NetHomeFinder.WebScraper.Service.Scrapers;
using NetHomeFinder.WebScraper.Service.Scrapers.Interface;

namespace NetHomeFinder.WebScraper.Module
{
	public class WebScrapperModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ScrapingManager>()
				.As<IScrapingManager>()
				.SingleInstance();

			builder.RegisterType<ImmobilienScoutScraperService>()
				.As<IScrapingService, ImmobilienScoutScraperService>()
				.SingleInstance();
		}
	}
}